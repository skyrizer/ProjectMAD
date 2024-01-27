using Azure.Core;
using CatViP_API.Models;
using CatViP_API.Repositories;
using CatViP_API.Repositories.Interfaces;
using CatViP_API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using CatViP_API.DTOs.AuthDTOs;
using AutoMapper;
using CatViP_API.DTOs.CatDTOs;

namespace CatViP_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseResult<User?>> Login(UserLoginRequestDTO userLoginDTO)
        {
            var resResult = new ResponseResult<User?>();
            var user = await _userRepository.AuthenticateUser(userLoginDTO);

            if (user == null)
            {
                resResult.IsSuccessful = false;
                return resResult;
            }

            resResult.Result = user;

            return resResult;
        }

        public async Task<ResponseResult<string>> CreateToken(User user)
        {
            var resResult = new ResponseResult<string>();

            var roleName = _userRepository.GetUserRoleName(user);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, roleName)
            };

            var values = new
            {
                TokenCreated = DateTime.Now,
                TokenExpires = DateTime.Now.AddDays(7)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!               ));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(claims: claims, expires: values.TokenExpires, signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            var updateUserTokenAction = await _userRepository.UpdateUserToken(user.Id, jwt, values.TokenCreated, values.TokenExpires);

            if (!updateUserTokenAction)
            {
                resResult.IsSuccessful = false;
                return resResult;
            }

            resResult.Result = jwt;

            return resResult;
        }

        public async Task<ResponseResult> DeleteToken(long userId)
        {
            var result = new ResponseResult();
            
            var deleteUserTokenAction = await _userRepository.DeleteUserToken(userId);

            if (!deleteUserTokenAction)
            {
                result.IsSuccessful = false;
            }

            return result;
        }

        public async Task<ResponseResult<User?>> GetUserFromJWTToken(string token)
        {
            var resResult = new ResponseResult<User?>();

            try
            {
                var jwt = new JwtSecurityToken(token);
                long userId = long.Parse(jwt.Claims.First(c => c.Type == _configuration.GetSection("Claims:Sid").Value).Value);

                resResult.Result = await _userRepository.GetUserById(userId);

                if (resResult.Result == null)
                {
                    resResult.IsSuccessful = false;
                    return resResult;
                }
            }
            catch (Exception)
            {
                resResult.IsSuccessful = false;
            }

            return resResult;
        }

        public ResponseResult VerifyToken(string token, User user)
        {
            var resResult = new ResponseResult();

            if (user.RememberToken != token)
            {
                resResult.IsSuccessful = false;
                resResult.ErrorMessage = "Invalid Token.";
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                resResult.IsSuccessful = false;
                resResult.ErrorMessage = "Token expired.";
            }

            return resResult;
        }

        public ResponseResult ValidateUsernameAndEmail(UserRegisterRequestDTO userRegisterDTO)
        {
            var resResult = new ResponseResult();

            if (_userRepository.CheckIfUsernameExist(userRegisterDTO.Username))
            {
                resResult.IsSuccessful = false;
                resResult.ErrorMessage = "Username is already registered in the system.";
            }
            else if (_userRepository.CheckIfEmailExist(userRegisterDTO.Email))
            {
                resResult.IsSuccessful = false;
                resResult.ErrorMessage = "Email is already registered in the system.";
            }

            return resResult;
        }

        public ResponseResult ValidateEmail(string email)
        {
            var resResult = new ResponseResult();
            
            if (_userRepository.CheckIfEmailExist(email))
            {
                resResult.IsSuccessful = false;
                resResult.ErrorMessage = "Email is already registered in the system.";
            }

            return resResult;
        }

        public ResponseResult ValidateRegisterRoleId(long RoleId)
        {
            var resResult = new ResponseResult();

            if (RoleId != 2 && RoleId != 4)
            {
                resResult.IsSuccessful = false;
                resResult.ErrorMessage = "Invalid Role Id";
            }

            return resResult;
        }

        public async Task<ResponseResult<User?>> StoreUser(UserRegisterRequestDTO userRegisterDTO)
        {
            var resResult = new ResponseResult<User?>();
            
            try
            {
                resResult.Result = await _userRepository.StoreUser(userRegisterDTO);
            }
            catch (Exception)
            {
                resResult.IsSuccessful = false;
            }

            return resResult;
        }

        public ResponseResult<string> GenerateForgotPasswordLink(string email)
        {
            var emailRes = new ResponseResult<string>();

            emailRes.IsSuccessful = _userRepository.CheckIfEmailExist(email);

            if (!emailRes.IsSuccessful)
            {
                emailRes.ErrorMessage = "Invalid Email";
                return emailRes;
            }

            var emailBytes = System.Text.Encoding.UTF8.GetBytes(email);

            var encryptedEmail = Convert.ToBase64String(emailBytes);

            emailRes.Result = $"http://{_httpContextAccessor.HttpContext!.Request.Host.Value}/auth/forgot-password?email={encryptedEmail}";

            return emailRes;
        }

        public async Task SendEmail(string email, string link)
        {
            var smtpClient = new SmtpClient("smtp.example.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("username@example.com", "password"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("username@example.com"),
                Subject = "Forgot Password",
                Body = $"<p>Please click the following link to reset your password:</p><a href='{link}'>Reset Password</a>",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task<ResponseResult<User?>> GetUser(long userId)
        {
            var resResult = new ResponseResult<User?>();

            resResult.Result = await _userRepository.GetUserById(userId);
            
            if (resResult.Result == null)
            {
                resResult.IsSuccessful = false;
                return resResult;
            }

            return resResult;
        }

        public async Task<ResponseResult> EditUserProfile(long userId, EditProfileDTO editProfileDTO)
        {
            var result = new ResponseResult();

            result.IsSuccessful = await _userRepository.UpdateUserProfile(userId, editProfileDTO);

            if (!result.IsSuccessful)
            {
                result.ErrorMessage = "fail to upate user profile";
                return result;
            }

            return result;
        }

        public async Task SendRecoverEmail(string email, string url)
        {
            var mail = "catvipa5@gmail.com";
            var pw = "uejw zktv cgje vcll";
            // Abc@12345

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            var sub = "To Recover Your CatVip Account";

            var message = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = sub,
                Body = $"Click the following link to recover your CatVip account: {url}",
                IsBodyHtml = true
            };

            message.To.Add(email);

            await client.SendMailAsync(message);
        }

        public async Task<ResponseResult> ResetPassword(ResetPasswordRequestDTO resetPasswordDTO)
        {
            var res = new ResponseResult();

            var encryptedEmailBytes = Convert.FromBase64String(resetPasswordDTO.Email);

            var email = System.Text.Encoding.UTF8.GetString(encryptedEmailBytes);

            res.IsSuccessful = await _userRepository.ResetUserPassword(email, resetPasswordDTO.Password);

            if (!res.IsSuccessful)
            {
                res.ErrorMessage = "fail to reset password.";
            }

            return res;
        }

        public AuthInfoDTO GetUserInfo(User user)
        {
            var userDTO = _mapper.Map<AuthInfoDTO>(user);
            userDTO.IsExpert = (user.RoleId == 3);
            userDTO.Following = _userRepository.GetUserFollowingCount(user.Id);
            userDTO.Follwer = _userRepository.GetUserFollowerCount(user.Id);

            if (userDTO.IsExpert)
            {
                userDTO.ExpertTips = _userRepository.GetExpertTipsCount(user.Id);
            }

            return userDTO;
        }
    }
}
