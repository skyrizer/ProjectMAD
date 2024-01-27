using CatViP_API.DTOs.PostDTOs;
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
    public class PostController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPostService _postService;
        private readonly ICatService _catService;

        public PostController(IAuthService authService, IPostService postService, ICatService catService)
        {
            _authService = authService;
            _postService = postService;
            _catService = catService;
        }

        [HttpGet("GetOwnPosts"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetOwnPosts()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var posts = _postService.GetOwnPosts(userResult.Result!);

            return Ok(posts);
        }

        [HttpGet("GetCatPosts/{catId}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetPostByCat(long catId)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var posts = _postService.GetPostsByCatId(userResult.Result!.Id, catId);

            return Ok(posts);
        }

        [HttpGet("GetPosts"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetPosts()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var posts = _postService.GetPosts(userResult.Result!);

            return Ok(posts);
        }

        [HttpGet("GetPost/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetPost(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                 return Unauthorized("invalid token");
            }

            var postRes  = _postService.GetPost(userResult.Result!.Id, Id);

            if (!postRes.IsSuccessful)
            {
                return BadRequest(postRes.ErrorMessage);
            }

            return Ok(postRes.Result!);
        }

        [HttpGet("GetPosts/{UserId}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetPostsByUserId(long UserId)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var posts = _postService.GetPostsByUserId(userResult.Result!.Id, UserId);

            return Ok(posts);
        }

        [HttpGet("GetPostComments/{postId}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetPostComments(int postId)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var postComments = _postService.GetPostComments(userResult.Result!.Id, postId);

            return Ok(postComments);
        }

        [HttpPut("ActPost"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> ActPost([FromBody] PostActionRequestDTO postActionDTO)
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

            var postActRes = await _postService.ActPost(userResult.Result!, postActionDTO);

            if (!postActRes.IsSuccessful)
            {
                return BadRequest("fail to act the post");
            }

            return Ok();
        }

        [HttpDelete("DeleteActPost/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> DeleteActPost(long Id)
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

            var delPostActRes = await _postService.DeleteActPost(userResult.Result!.Id, Id);

            if (!delPostActRes.IsSuccessful)
            {
                return BadRequest("fail to delete act the post");
            }

            return Ok();
        }

        [HttpPost("CreateComment"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> CreateComment([FromBody] CommentRequestDTO commentRequestDTO)
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

            var postActRes = await _postService.CommentPost(userResult.Result!, commentRequestDTO);

            if (!postActRes.IsSuccessful)
            {
                return BadRequest(postActRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet("GetPostTypes"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> GetPostTypes()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var result = _postService.GetPostTypes(userResult.Result!);

            return Ok(result);
        }

        [HttpPost("CreatePost"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> CreatePost([FromBody]PostRequestDTO createPostDTO)
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

            var createPostResult = await _postService.CreatePost(userResult.Result!, createPostDTO);

            if (!createPostResult.IsSuccessful)
            {
                return BadRequest(createPostResult.ErrorMessage);
            }

            return Ok();
        }

        [HttpPut("EditPost/{Id}"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> EditPost(long Id, [FromBody] EditPostRequestDTO editPostRequestDTO)
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

            var checkPostRes = _postService.CheckIfPostExist(userResult.Result!.Id, Id);

            if (!checkPostRes.IsSuccessful)
            {
                return BadRequest(checkPostRes.ErrorMessage);
            }

            var postActRes = await _postService.EditPost(Id, editPostRequestDTO);

            if (!postActRes.IsSuccessful)
            {
                return BadRequest("fail to act the post");
            }

            return Ok();
        }

        [HttpDelete("DeletePost/{Id}"), Authorize(Roles = "System Admin,Cat Owner,Cat Expert")]
        public async Task<IActionResult> DeletePost(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            if (userResult.Result!.RoleId != 1)
            {
                var checkPostRes = _postService.CheckIfPostExist(userResult.Result!.Id, Id);

                if (!checkPostRes.IsSuccessful)
                {
                    return BadRequest(checkPostRes.ErrorMessage);
                }
            }

            var delPostRes = await _postService.DeletePost(Id);

            if (!delPostRes.IsSuccessful)
            {
                return BadRequest(delPostRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpPost("ReportPost"), Authorize(Roles = "Cat Owner,Cat Expert")]
        public async Task<IActionResult> ReportPost([FromBody] PostReportRequestDTO reportPostDTO)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var reportPostRes = await _postService.ReportPost(reportPostDTO, userResult.Result!.Id);

            if (!reportPostRes.IsSuccessful)
            {
                return BadRequest(reportPostRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet("GetReportedPosts"), Authorize(Roles = "System Admin")]
        public async Task<IActionResult> GetReportedPosts()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var reportPosts = _postService.GetReportedPosts();

            return Ok(reportPosts);
        }

        [HttpGet("GetReportedPostDetails/{Id}"), Authorize(Roles = "System Admin")]
        public async Task<IActionResult> GetReportedPostDetails(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var reportPostDetails = _postService.GetReportedPostDetails(Id);

            return Ok(reportPostDetails);
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

            var checkPostRes = _postService.CheckIfCommentExist(userResult.Result!.Id, Id);

            if (!checkPostRes.IsSuccessful)
            {
                return BadRequest(checkPostRes.ErrorMessage);
            }

            var delPostRes = await _postService.DeleteComment(Id);

            if (!delPostRes.IsSuccessful)
            {
                return BadRequest(delPostRes.ErrorMessage);
            }

            return Ok();
        }
    }
}
