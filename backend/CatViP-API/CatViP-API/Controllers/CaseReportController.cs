using CatViP_API.DTOs.CaseReportDTOs;
using CatViP_API.DTOs.PostDTOs;
using CatViP_API.Services;
using CatViP_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatViP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseReportController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICaseReportService _caseReportService;

        public CaseReportController(IAuthService authService, ICaseReportService caseReportService)
        {
            _authService = authService;
            _caseReportService = caseReportService;
        }

        [HttpGet("GetOwnCaseReports"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetOwnCaseReports()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var cases = _caseReportService.GetOwnCaseReports(userResult.Result!.Id);

            return Ok(cases);
        }

        [HttpGet("GetOwnCaseReport/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetOwnCaseReport(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var caseRes = _caseReportService.GetOwnCaseReport(Id, userResult.Result!);

            if (!caseRes.IsSuccessful)
            {
                return BadRequest(caseRes.ErrorMessage);
            }

            return Ok(caseRes.Result);
        }

        [HttpGet("GetNearByCaseReports"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetNearByCaseReports()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var cases = _caseReportService.GetNearByCaseReports(userResult.Result!);

            return Ok(cases);
        }

        [HttpGet("GetNearByCaseReportsCount"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetNearByCaseReportsCount()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var count = _caseReportService.GetNearByCaseReportsCount(userResult.Result!);

            return Ok(count);
        }

        [HttpGet("GetNearByCaseReport/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetNearByCaseReport(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var caseRes = _caseReportService.GetNearByCaseReport(Id, userResult.Result!);

            if (!caseRes.IsSuccessful)
            {
                return BadRequest(caseRes.ErrorMessage);
            }

            return Ok(caseRes.Result);
        }

        [HttpPut("SettleCaseReport/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> SettleCaseReport(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var checkIsReportExistRes = _caseReportService.CheckIsReportExist(userResult.Result!.Id, Id);

            if (!checkIsReportExistRes.IsSuccessful)
            {
                return BadRequest(checkIsReportExistRes.ErrorMessage);
            }

            var res = await _caseReportService.SettleCaseReport(Id);

            if (!res.IsSuccessful)
            {
                return BadRequest(res.ErrorMessage);
            }

            return Ok();
        }

        [HttpDelete("RevokeCaseReport/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> RevokeCaseReport(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var checkIsReportExistRes = _caseReportService.CheckIsReportExist(userResult.Result!.Id, Id);

            if (!checkIsReportExistRes.IsSuccessful)
            {
                return BadRequest(checkIsReportExistRes.ErrorMessage);
            }

            var res = await _caseReportService.RevokeCaseReport(Id);

            if (!res.IsSuccessful)
            {
                return BadRequest(res.ErrorMessage);
            }

            return Ok();
        }

        [HttpPost("CreateCaseReport"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> CreateCaseReport([FromBody] CaseReportRequestDTO caseReportRequestDTO)
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

            var createPostResult = await _caseReportService.CreateCaseReport(userResult.Result!.Id, caseReportRequestDTO);

            if (!createPostResult.IsSuccessful)
            {
                return BadRequest(createPostResult.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet("GetCaseReportComments/{caseReportId}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetCaseReportComments(int caseReportId)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var comments = _caseReportService.GetCaseReportComments(userResult.Result!.Id, caseReportId);

            return Ok(comments);
        }

        [HttpPost("CreateComment"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> CreateComment([FromBody] CatCaseReportCommentRequestDTO commentRequestDTO)
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

            var postActRes = await _caseReportService.CommentCaseReport(userResult.Result!, commentRequestDTO);

            if (!postActRes.IsSuccessful)
            {
                return BadRequest(postActRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpDelete("DeleteComment/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> DeleteComment(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var checkPostRes = _caseReportService.CheckIfCaseReportExist(userResult.Result!.Id, Id);

            if (!checkPostRes.IsSuccessful)
            {
                return BadRequest(checkPostRes.ErrorMessage);
            }

            var delPostRes = await _caseReportService.DeleteComment(Id);

            if (!delPostRes.IsSuccessful)
            {
                return BadRequest(delPostRes.ErrorMessage);
            }

            return Ok();
        }
    }
}
