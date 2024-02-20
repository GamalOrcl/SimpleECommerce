using SimpleECommerce.Core.Models;

namespace SimpleECommerce.Core.Interfaces
{
    public interface IAuthService
    {
        Task<(int, string)> Registeration(RegistrationModel model);
        Task<TokenViewModel> Login(LoginModel model);
        Task<TokenViewModel> GetRefreshToken(GetRefreshTokenViewModel model);

        Task<IdentityResponseModel> ChangePassword(ChangePasswordModel model);

        Task<IdentityResponseModel> UpdateUserInfo(UpdateUserInfoModel model);

    }
}
