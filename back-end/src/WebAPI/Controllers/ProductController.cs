using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;

        public ProductController(IProductService productService, ICommentService commentService)
        {
            _productService = productService;
            _commentService = commentService;
        }

        [HttpGet("get-all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var allProducts = await _productService.GetAllProductsAsync();
                return Ok(ApiResult.Success(allProducts!));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult.Failure(null!, ex.Message));
            }
        }

        [HttpGet("get/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(productId);

                return Ok(ApiResult.Success(product!));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult.Failure(null!, ex.Message));
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto product)
        {
            try
            {
                product.CreatedBy = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                await _productService.AddProductAsync(product);
                return Ok(ApiResult.Success());
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult.Failure(null!, ex.Message));
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductDto product)
        {
            try
            {
                product.UpdateBy = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                await _productService.UpdateProductAsync(product);
                return Ok(ApiResult.Success());
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult.Failure(null!, ex.Message));
            }
        }

        [HttpDelete("delete/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            try
            {
                await _productService.DeleteProductAsync(productId);
                return Ok(ApiResult.Success());
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult.Failure(null!, ex.Message));
            }
        }
    }
}
