using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWebApi.DTOs;
using WebApi.Services;

namespace ShopWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto productDto)
        {
           
            int userId = GetAuthenticatedUserId();

            try
            {
                await _productService.CreateProduct(productDto, userId);
                return Ok(new { message = "Product created successfully." });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{productId}")]
        public IActionResult GetProductById(int productId)
        {
           
            int userId = GetAuthenticatedUserId();

            var product = _productService.GetProductById(productId, userId);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPut]
        public IActionResult UpdateProduct([FromForm] ProductDto productDto)
        {
            
            int userId = GetAuthenticatedUserId();

            try
            {
                _productService.UpdateProduct(productDto, userId);
                return Ok(new { message = "Product updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            
            int userId = GetAuthenticatedUserId();

            try
            {
                _productService.DeleteProduct(productId, userId);
                return Ok(new { message = "Product deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private int GetAuthenticatedUserId()
        {
           
            return 0;
        }
    }
}
