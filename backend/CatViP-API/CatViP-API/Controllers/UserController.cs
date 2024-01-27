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
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserController(IAuthService authService, IUserService userService)
        {
            this._authService = authService;
            this._userService = userService;
        }

        [HttpGet("SearchUser"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> SearchUsername([FromQuery] string Name)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var users = _userService.SearchByUsenameOrFullName(Name.Trim(), userResult.Result!.Id);

            return Ok(users);
        }

        [HttpPost("FollowUser/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> FollowUser(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var followRes = await _userService.FollowUser(userResult.Result!.Id, Id);

            if (!followRes.IsSuccessful)
            {
                return BadRequest(followRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpDelete("UnfollowUser/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> UnfollowUser(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var unfollowRes = await _userService.UnfollowUser(userResult.Result!.Id, Id);

            if (!unfollowRes.IsSuccessful)
            {
                return BadRequest(unfollowRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet("GetSearchUserInfo/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetSearchUserInfo(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var userRes = await _userService.GetSearchUserInfo(userResult.Result!.Id, Id);

            if (!userRes.IsSuccessful)
            {
                return BadRequest(userRes.ErrorMessage);
            }

            return Ok(userRes.Result!);
        }

        [HttpGet("GetUserInfoById/{Id}"), Authorize(Roles = "System Admin,Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetUserInfoById(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var userRes = await _userService.GetUserInfoById(Id);

            if (!userRes.IsSuccessful)
            {
                return BadRequest(userRes.ErrorMessage);
            }

            return Ok(userRes.Result!);
        }
    }
}
