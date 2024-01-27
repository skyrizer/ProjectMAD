using CatViP_API.DTOs.ProductDTOs;
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
    public class ProductController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IProductService _productService;

        public ProductController(IAuthService authService, IProductService productService)
        {
            this._authService = authService;
            _productService = productService;
        }

        [HttpGet("GetProductTypes"), Authorize(Roles = "Cat Product Seller")]
        public async Task<IActionResult> GetProductTypes()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var productTypes = _productService.GetProductTypes();

            return Ok(productTypes);
        }

        [HttpGet("GetProducts"), Authorize(Roles = "Cat Product Seller")]
        public async Task<IActionResult> GetProducts()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var products = _productService.GetProducts(userResult.Result!.Id);

            return Ok(products);
        }

        [HttpGet("GetProduct/{Id}"), Authorize(Roles = "Cat Product Seller")]
        public async Task<IActionResult> GetProductById(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var checkProductRes = _productService.CheckProductExist(userResult.Result!.Id, Id);

            if (!checkProductRes.IsSuccessful)
            {
                return BadRequest(checkProductRes.ErrorMessage);
            }

            var products = _productService.GetProductById(Id);

            return Ok(products);
        }

        [HttpPost("Store"), Authorize(Roles = "Cat Product Seller")]
        public async Task<IActionResult> StoreProduct([FromBody]ProductRequestDTO productRequestDTO)
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

            var productRes = await _productService.StoreProduct(userResult.Result!.Id, productRequestDTO);

            if (!productRes.IsSuccessful)
            {
                return BadRequest(productRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpPut("Edit/{Id}"), Authorize(Roles = "Cat Product Seller")]
        public async Task<IActionResult> EditProduct(long Id, [FromBody] ProductEditRequestDTO productRequestDTO)
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

            var checkProductRes = _productService.CheckProductExist(userResult.Result!.Id, Id);

            if (!checkProductRes.IsSuccessful)
            {
                return BadRequest(checkProductRes.ErrorMessage);
            }

            var productRes = await _productService.EditProduct(Id, productRequestDTO);

            if (!productRes.IsSuccessful)
            {
                return BadRequest(productRes.ErrorMessage);
            }

            return Ok();
        }

        [HttpDelete("Delete/{Id}"), Authorize(Roles = "Cat Product Seller")]
        public async Task<IActionResult> DeleteProduct(long Id)
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            string token = authorizationHeader.Substring("Bearer ".Length);

            var userResult = await _authService.GetUserFromJWTToken(token);

            if (!userResult.IsSuccessful)
            {
                return Unauthorized("invalid token");
            }

            var checkProductRes = _productService.CheckProductExist(userResult.Result!.Id, Id);

            if (!checkProductRes.IsSuccessful)
            {
                return BadRequest(checkProductRes.ErrorMessage);
            }

            var productRes = await _productService.DeleteProduct(Id);

            if (!productRes.IsSuccessful)
            {
                return BadRequest(productRes.ErrorMessage);
            }

            return Ok();
        }
    }
}
