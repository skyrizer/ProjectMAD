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
    public class AnalysisController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAnalysisService _analysisService;

        public AnalysisController(IAuthService authService, IAnalysisService analysisService)
        {
            _authService = authService;
            _analysisService = analysisService;
        }

        [HttpGet("GetPostsAndExpertTipsCount"), Authorize(Roles = "System Admin")]
        public async Task<IActionResult> GetPostsAndExpertTipsCount([FromQuery]string query)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var countRes = _analysisService.GetPostsAndExpertTipsCount(query);

            if (!countRes.IsSuccessful)
            {
                return BadRequest(countRes.ErrorMessage);
            }

            return Ok(countRes.Result!);
        }

        [HttpGet("GetUsersAndProductsCount"), Authorize(Roles = "System Admin")]
        public async Task<IActionResult> GetUsersAndProductsCount()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var count = _analysisService.GetUsersAndProductsCount();

            return Ok(count);
        }

        [HttpGet("GetMissingCatsCount"), Authorize(Roles = "System Admin")]
        public async Task<IActionResult> GetMissingCatsCount([FromQuery] string query, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var countRes = _analysisService.GetMissingCatsCount(query, startDate, endDate);

            if (!countRes.IsSuccessful)
            {
                return BadRequest(countRes.ErrorMessage);
            }

            return Ok(countRes.Result!);
        }
    }
}
