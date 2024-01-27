using CatViP_API.DTOs.AuthDTOs;
using CatViP_API.Models;
using CatViP_API.Repositories.Interfaces;
using CatViP_API.Services;
using CatViP_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatViP_API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginRequestDTO userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lgoinResult = await _authService.Login(userLogin);

            if (!lgoinResult.IsSuccessful)
                return Unauthorized();

            // get token
            var tokenResult = await _authService.CreateToken(lgoinResult.Result!);

            return Ok(tokenResult.Result);
        }

        [HttpPut("refresh")]
        public async Task<IActionResult> RefreshToken([FromHeader]string token)
        {
            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
                return Unauthorized();

            var verifyTokenResult = _authService.VerifyToken(token, userResult.Result!);

            if (!verifyTokenResult.IsSuccessful)
            {
                return Unauthorized(verifyTokenResult.ErrorMessage);
            }

            var newTokenResult = await _authService.CreateToken(userResult.Result!);

            return Ok(newTokenResult.Result);
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> Logout([FromHeader]string token)
        {
            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
                return Unauthorized();

            var res = _authService.VerifyToken(token, userResult.Result!);

            if (!res.IsSuccessful)
                return Unauthorized(res.ErrorMessage);

            await _authService.DeleteToken(userResult.Result!.Id);

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterRequestDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resResult = _authService.ValidateRegisterRoleId(userRegisterDTO.RoleId);

            if (!resResult.IsSuccessful)
            {
                return BadRequest(resResult.ErrorMessage);
            }

            resResult = _authService.ValidateUsernameAndEmail(userRegisterDTO);

            if (!resResult.IsSuccessful)
            { 
                return Conflict(resResult.ErrorMessage);
            }

            var user = await _authService.StoreUser(userRegisterDTO);

            if (!user.IsSuccessful)
            {
                return BadRequest("fail to register");
            }

            var newTokenResult = await _authService.CreateToken(user.Result!);

            return Ok(newTokenResult.Result);
        }

        [HttpPut("editProfile"), Authorize(Roles = "System Admin,Cat Owner,Cat Expert,Cat Product Seller")]
        public async Task<IActionResult> EditProfileRequest(EditProfileDTO editProfileDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized(userResult.ErrorMessage);
            }

            var editUserResult = await _authService.EditUserProfile(userResult.Result!.Id, editProfileDTO);

            if (!editUserResult.IsSuccessful)
            {
                return BadRequest(editUserResult.ErrorMessage);
            }

            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                {
                    return BadRequest("Invalid email address");
                }

                var linkRes = _authService.GenerateForgotPasswordLink(email);

                if (!linkRes.IsSuccessful)
                {
                    return BadRequest(linkRes.ErrorMessage);
                }

                await _authService.SendRecoverEmail(email, linkRes.Result!);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _authService.ResetPassword(resetPasswordDTO);

            if (!res.IsSuccessful)
            {
                return BadRequest(res.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet("GetUserInfo"), Authorize(Roles = "System Admin,Cat Owner,Cat Expert,Cat Product Seller")]
        public async Task<IActionResult> GetUserInfo()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized(userResult.ErrorMessage);
            }

            var userProfileDTO = _authService.GetUserInfo(userResult.Result!);

            return Ok(userProfileDTO);
        }
    }
}
