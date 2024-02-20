using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SimpleECommerce.Core.Consts;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.Core.Interfaces;
using SimpleECommerce.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
 

namespace SimpleECommerce.Core.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        } 


        public async Task<(int, string)> Registeration(RegistrationModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return (0, "Email already exists");

            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return (0, "User already exists"); 

            if (_unitOfWork.Repository<ApplicationUser>().IsExist(x => x.Mobile == model.Mobile))
                return (0, "Mobile already exists");

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Mobile = model.Mobile 
            };
            var createUserResult = await _userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
                return (0, "User creation failed! Please check user details and try again.");

            if (!await _roleManager.RoleExistsAsync(model.RoleId))
                await _roleManager.CreateAsync(new IdentityRole(model.RoleId));

            if (await _roleManager.RoleExistsAsync(model.RoleId))
                await _userManager.AddToRoleAsync(user, model.RoleId);

            return (1, "User created successfully!");
        }

        public async Task<TokenViewModel> Login(LoginModel model)
        {
            TokenViewModel _TokenViewModel = new();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                _TokenViewModel.StatusCode = 0;
                _TokenViewModel.StatusMessage = "Invalid username";
                return _TokenViewModel;
            }
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _TokenViewModel.StatusCode = 0;
                _TokenViewModel.StatusMessage = "Invalid password";
                return _TokenViewModel;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            _TokenViewModel.AccessToken = GenerateToken(authClaims);
            _TokenViewModel.RefreshToken = GenerateRefreshToken();
            _TokenViewModel.StatusCode = 1;
            _TokenViewModel.StatusMessage = "Success";

            var _RefreshTokenValidityInDays = Convert.ToInt64(_configuration["JWTKey:RefreshTokenValidityInDays"]);
            user.RefreshToken = _TokenViewModel.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_RefreshTokenValidityInDays);
            await _userManager.UpdateAsync(user);


            return _TokenViewModel;
        }

        public async Task<TokenViewModel> GetRefreshToken(GetRefreshTokenViewModel model)
        {
            TokenViewModel _TokenViewModel = new();
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);
            string username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                _TokenViewModel.StatusCode = 0;
                _TokenViewModel.StatusMessage = "Invalid access token or refresh token";
                return _TokenViewModel;
            }




            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }




            var newAccessToken = GenerateToken(authClaims);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            _TokenViewModel.StatusCode = 1;
            _TokenViewModel.StatusMessage = "Success";
            _TokenViewModel.AccessToken = newAccessToken;
            _TokenViewModel.RefreshToken = newRefreshToken;
            return _TokenViewModel;
        }


        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
            var _TokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInHour"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Audience = _configuration["JWTKey:ValidAudience"],
                //Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }




         





        public async Task<IdentityResponseModel> UpdateUserInfo(UpdateUserInfoModel model)
        {
            var responseStatus = new IdentityResponseModel();
            var responseErrors = new List<IdentityErrorResponseModel>();

            var user = _userManager.Users.Where(u => u.Id == model.UserName).SingleOrDefault();
            if (user == null)
            {
                responseErrors.Add(new IdentityErrorResponseModel()
                {
                    ErrorCode = "777",
                    ErrorDescription = $"The user {model.UserName} does not exist."
                });
                responseStatus.IsSuccessful = false;
                responseStatus.errors = responseErrors;
                return responseStatus;
            }

            // Verify current user is an administrator or the target user whose password 
            // is being modified.
            if ((!await _userManager.IsInRoleAsync(user, StaticUserRoles.ADMIN) && (user.UserName != model.UserName)))
            {
                responseErrors.Add(new IdentityErrorResponseModel()
                {
                    ErrorCode = "666",
                    ErrorDescription = $"Only an Administrator or the user {model.UserName} can perform this operation."
                });
                responseStatus.IsSuccessful = false;
                responseStatus.errors = responseErrors;
                return responseStatus;
            }


            var rslt = await _userManager.UpdateAsync(user);
            if (!rslt.Succeeded)
            {
                foreach (IdentityError err in rslt.Errors)
                {
                    responseErrors.Add(new IdentityErrorResponseModel()
                    {
                        ErrorCode = err.Code,
                        ErrorDescription = err.Description
                    });
                }
            }
            responseStatus.errors = responseErrors;
            responseStatus.IsSuccessful = !responseStatus.errors.Any();


            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;


            return responseStatus;
        }


          
        public async Task<IdentityResponseModel> ChangePassword(ChangePasswordModel model)
        {
            var responseStatus = new IdentityResponseModel();
            var responseErrors = new List<IdentityErrorResponseModel>();

            var user = _userManager.Users.Where(u => u.UserName == model.UserName).SingleOrDefault();
            if (user == null)
            {
                responseErrors.Add(new IdentityErrorResponseModel()
                {
                    ErrorCode = "777",
                    ErrorDescription = $"The user {model.UserName} does not exist."
                });
                responseStatus.IsSuccessful = false;
                responseStatus.errors = responseErrors;
                return responseStatus;
            }

            // Verify current user is an administrator or the target user whose password 
            // is being modified.
            if ((!await _userManager.IsInRoleAsync(user, StaticUserRoles.ADMIN) && (user.UserName != model.UserName)))
            {
                responseErrors.Add(new IdentityErrorResponseModel()
                {
                    ErrorCode = "666",
                    ErrorDescription = $"Only an Administrator or the user {model.UserName} can perform this operation."
                });
                responseStatus.IsSuccessful = false;
                responseStatus.errors = responseErrors;
                return responseStatus;
            }

            // Verify current password matches specified password.
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            if (passwordHasher.VerifyHashedPassword(user,
                user.PasswordHash, model.OriginalPassword) == PasswordVerificationResult.Failed)
            {
                //_logger.LogInformation(
                //    "The original and specified user passwords do not match.");

                // Bump up the access failed attempts for the user.
                await _userManager.AccessFailedAsync(user);

                responseErrors.Add(new IdentityErrorResponseModel()
                {
                    ErrorCode = "555",
                    ErrorDescription = "The original and specified user passwords do not match."
                });
                responseStatus.IsSuccessful = false;
                responseStatus.errors = responseErrors;
                return responseStatus;
            }

            // Save new hashed password to user record.
            user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);

            var rslt = await _userManager.UpdateAsync(user);
            if (!rslt.Succeeded)
            {
                foreach (IdentityError err in rslt.Errors)
                {
                    responseErrors.Add(new IdentityErrorResponseModel()
                    {
                        ErrorCode = err.Code,
                        ErrorDescription = err.Description
                    });
                }
            }
            responseStatus.errors = responseErrors;
            responseStatus.IsSuccessful = !responseStatus.errors.Any();



            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);


            return responseStatus;
        }





         



















        public async Task<ApplicationUser> GetUserAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<bool> ValidatePasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> AddClaimAsync(ApplicationUser user, Claim claim)
        {
            var result = await _userManager.AddClaimAsync(user, claim);
            return result.Succeeded;
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<bool> RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            var result = await _userManager.RemoveClaimAsync(user, claim);
            return result.Succeeded;
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }


        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }









    }
}
