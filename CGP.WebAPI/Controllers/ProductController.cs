using CGP.Application.Interfaces;
using CGP.Contract.DTO.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _productService.GetProductsAsync();
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("CreateProduct")]
        [Authorize(Policy = "ArtisanPolicy")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto request)
        {
            if (request == null || request.Image == null)
            {
                return BadRequest(new { Error = 1, Message = "Invalid product data." });
            }
            var result = await _productService.CreateProduct(request);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
