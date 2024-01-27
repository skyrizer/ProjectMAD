using CatViP_API.DTOs.CatDTOs;
using CatViP_API.Models;
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
    public class CatController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICatService _catService;

        public CatController(IAuthService authService, ICatService catService)
        {
            this._authService = authService;
            _catService = catService;
        }

        [HttpGet("GetCats"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetCats()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var cats = _catService.GetCats(userResult.Result!.Id);

            return Ok(cats);
        }

        [HttpGet("GetCats/{UserId}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetCats(long UserId)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var cats = _catService.GetCats(UserId);

            return Ok(cats);
        }

        [HttpGet("GetCat/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetCat(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var cats = _catService.GetCat(Id);

            return Ok(cats);
        }

        [HttpPost("StoreCat"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> StoreCat([FromBody] CatRequestDTO createCatRequestDTO)
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

            var catRes = await _catService.StoreCat(userResult.Result!.Id, createCatRequestDTO);

            if (!catRes.IsSuccessful)
            {
                return BadRequest(catRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpPut("EditCat/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> EditCat(long Id, [FromBody] CatRequestDTO editCatRequestDTO)
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

            var checkCatRes = _catService.CheckIfCatExist(userResult.Result!.Id, Id);

            if (!checkCatRes.IsSuccessful)
            {
                return BadRequest(checkCatRes.ErrorMessage);
            }

            var catRes = await _catService.EditCat(Id, editCatRequestDTO);

            if (!catRes.IsSuccessful)
            {
                return BadRequest(catRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpDelete("DeleteCat/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> DeleteCat(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var checkCatRes = _catService.CheckIfCatExist(userResult.Result!.Id, Id);

            if (!checkCatRes.IsSuccessful)
            {
                return BadRequest(checkCatRes.ErrorMessage);
            }

            var catRes = await _catService.DeleteCat(Id);

            if (!catRes.IsSuccessful)
            {
                return BadRequest(catRes.ErrorMessage);
            }

            return Ok();
        }
    }
}
