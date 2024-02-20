using Microsoft.AspNetCore.Mvc;
using SimpleECommerce.Core.Interfaces;
using SimpleECommerce.Core.Models; 

namespace SimpleECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    { 
        private readonly IRoleService _roleService;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IRoleService roleService, IAuthService authService, ILogger<AuthenticationController> logger)
        { 
            _roleService = roleService;
            _authService = authService;
            _logger = logger;
        }


        [HttpPost]
        [Route("registeration")]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status, message) = await _authService.Registeration(model);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return CreatedAtAction(nameof(Register), model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var result = await _authService.Login(model);
                if (result.StatusCode == 0)
                    return BadRequest(result.StatusMessage);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }





        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserInfoModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("All fields is required");
                var result = await _authService.UpdateUserInfo(model);
                if (!result.IsSuccessful)
                    return BadRequest(result.errors);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("All fields is required");
                var result = await _authService.ChangePassword(model);
                if (!result.IsSuccessful)
                    return BadRequest(result.errors);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }












        //[HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> UpdateUser(string id, UpdateUserModel model)
        //{
        //    var user = await _authService.GetUserByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    // Update the user properties
        //    user.Email = model.Email;
        //    user.FirstName = model.FirstName;
        //    user.LastName = model.LastName;
        //    user.Mobile = model.Mobile;

        //    // Save the changes
        //    var result = await _authService.UpdateUserAsync(user);

        //    if (result.Succeeded)
        //    {
        //        return Ok("User updated successfully");
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}

        //[HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteUser(string id)
        //{
        //    var user = await _authService.GetUserByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var result = await _authService.DeleteUserAsync(user);

        //    if (result.Succeeded)
        //    {
        //        return Ok("User deleted successfully");
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}

        //[HttpPost("{id}/roles")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AddToRole(string id, AddToRoleModel model)
        //{
        //    var user = await _authService.GetUserByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var roleExists = await _roleService.RoleExistsAsync(model.RoleName);

        //    if (!roleExists)
        //    {
        //        return BadRequest("Role does not exist");
        //    }

        //    var result = await _authService.AddToRoleAsync(user, model.RoleName);

        //    if (result.Succeeded)
        //    {
        //        return Ok("User added to role successfully");
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}

        //[HttpDelete("{id}/roles")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> RemoveFromRole(string id, RemoveFromRoleModel model)
        //{
        //    var user = await _authService.GetUserByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var roleExists = await _roleService.RoleExistsAsync(model.RoleName);

        //    if (!roleExists)
        //    {
        //        return BadRequest("Role does not exist");
        //    }

        //    var result = await _authService.RemoveFromRoleAsync(user, model.RoleName);

        //    if (result.Succeeded)
        //    {
        //        return Ok("User removed from role successfully");
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}

        //// Additional methods for user management

        //[HttpGet("{id}/roles")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetRoles(string id)
        //{
        //    var user = await _authService.GetUserByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var roles = await _authService.GetRolesAsync(user);

        //    return Ok(roles);
        //}

        //[HttpPost("{id}/claims")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AddClaim(string id, AddClaimModel model)
        //{
        //    var user = await _authService.GetUserByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var claim = new Claim(model.ClaimType, model.ClaimValue);

        //    var success = await _authService.AddClaimAsync(user, claim);

        //    if (success)
        //    {
        //        return Ok("Claim added successfully");
        //    }
        //    else
        //    {
        //        return BadRequest("Failed to add claim");
        //    }
        //}

        //[HttpGet("{id}/claims")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetClaims(string id)
        //{
        //    var user = await _authService.GetUserByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var claims = await _authService.GetClaimsAsync(user);

        //    return Ok(claims);
        //}

        //[HttpDelete("{id}/claims")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> RemoveClaim(string id, RemoveClaimModel model)
        //{
        //    var user = await _authService.GetUserByIdAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var claim = new Claim(model.ClaimType, model.ClaimValue);

        //    var success = await _authService.RemoveClaimAsync(user, claim);

        //    if (success)
        //    {
        //        return Ok("Claim removed successfully");
        //    }
        //    else
        //    {
        //        return BadRequest("Failed to remove claim");
        //    }
        //}

        //[HttpGet("roles/{role}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetUsersInRole(string role)
        //{
        //    var users = await _authService.GetUsersInRoleAsync(role);

        //    return Ok(users);
        //}












    }
}
