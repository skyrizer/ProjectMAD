using CatViP_API.DTOs.ExpertDTOs;
using CatViP_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace CatViP_API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class ExpertController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IExpertService _expertService;
        public ExpertController(IAuthService authService, IExpertService expertService)
        {
            _authService = authService;
            _expertService = expertService;
        }

        [HttpGet("GetLastestApplication"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetLastestApplication()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var application = _expertService.GetLastestApplication(userResult.Result!.Id);

            if (application == null)
            {
                return NoContent();
            }

            return Ok(application);
        }

        [HttpPost("ApplyAsExpert"), Authorize(Roles = "Cat Owner")]
        public async Task<IActionResult> ExpertApplication([FromBody] ExpertApplicationRequestDTO expertApplicationRequestDTO)
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
                return Unauthorized("invalid token");
            }

            var applicationRes = await _expertService.ApplyAsExpert(userResult.Result!.Id, expertApplicationRequestDTO);

            if (!applicationRes.IsSuccessful)
            {
                return BadRequest(applicationRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpDelete("RevokeApplication/{Id}"), Authorize(Roles = "Cat Owner")]
        public async Task<IActionResult> RevokeApplication(long Id)
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
                return Unauthorized("invalid token");
            }

            var checkApplicationRes = _expertService.CheckIfPendingApplicationExist(userResult.Result!.Id, Id);

            if (!checkApplicationRes.IsSuccessful)
            {
                return BadRequest(checkApplicationRes.ErrorMessage);
            }

            var revokeApplicationRes = await _expertService.RevokeApplication(Id);

            if (!revokeApplicationRes.IsSuccessful)
            {
                return BadRequest(revokeApplicationRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet("GetPendingApplications"), Authorize(Roles = "System Admin")]
        public async Task<IActionResult> GetPendingApplications()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var applications = _expertService.GetPendingApplications();

            return Ok(applications);
        }

        [HttpGet("GetApplications"), Authorize(Roles = "System Admin")]
        public async Task<IActionResult> GetApplications()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var applications = _expertService.GetApplications();

            return Ok(applications);
        }

        [HttpGet("GetApplicationById/{Id}"), Authorize(Roles = "System Admin")]
        public async Task<IActionResult> GetApplicationById(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var application = _expertService.GetApplicationById(Id);

            return Ok(application);
        }

        [HttpPut("UpateExpertApplicationStatus"), Authorize(Roles = "System Admin")]
        public async Task<IActionResult> UpateExpertApplicationStatusAsync(ExpertApplicationActionRequestDTO expertApplicationActionRequestDTO)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var checkApplicationRes = _expertService.CheckIfPendingApplicationExist(expertApplicationActionRequestDTO.ApplictionId);

            if (!checkApplicationRes.IsSuccessful)
            {
                return BadRequest(checkApplicationRes.ErrorMessage);
            }

            var updateStatusRes = await _expertService.UpdateApplicationStatus(expertApplicationActionRequestDTO);

            if (!updateStatusRes.IsSuccessful)
            {
                return BadRequest(updateStatusRes.ErrorMessage);
            }

            return Ok();
        }
    }
}
