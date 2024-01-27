using CatViP_API.DTOs.AuthDTOs;
using CatViP_API.Models;

namespace CatViP_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseResult<User?>> Login(UserLoginRequestDTO userLoginDTO);
        Task<ResponseResult<User?>> GetUser(long userId);
        Task<ResponseResult<string>> CreateToken(User user);
        Task<ResponseResult> DeleteToken(long userId);
        Task<ResponseResult<User?>> GetUserFromJWTToken(string token);
        ResponseResult VerifyToken(string token, User userId);
        ResponseResult ValidateUsernameAndEmail(UserRegisterRequestDTO userRegisterDTO);
        ResponseResult ValidateRegisterRoleId(long RoleId);
        Task<ResponseResult<User?>> StoreUser(UserRegisterRequestDTO userRegisterDTO);
        ResponseResult ValidateEmail(string email);
        Task<ResponseResult> EditUserProfile(long userId, EditProfileDTO editProfileDTO);
        ResponseResult<string> GenerateForgotPasswordLink(string email);
        Task SendRecoverEmail(string email, string url);
        Task<ResponseResult> ResetPassword(ResetPasswordRequestDTO resetPasswordDTO);
        AuthInfoDTO GetUserInfo(User user);
    }
}
