using CGP.Application.Interfaces;
using CGP.Contract.DTO.Product;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Enums;
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

        [HttpGet("SearchProducts")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? search, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] decimal from = 0, [FromQuery] decimal to = 1000000, [FromQuery] SortOrder sortOrder = SortOrder.asc)
        {
            var result = await _productService.SearchProducts(search, pageIndex, pageSize, from, to, sortOrder.ToString());
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetProductByProductId/{productId}")]
        public async Task<IActionResult> GetProductByProductId(Guid productId)
        {
            if(productId == Guid.Empty)
            {
                return BadRequest(new { Error = 1, Message = "Mã sản phẩm không hợp lệ." });
            }
            var result = await _productService.GetProductByProductId(productId);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetProductsByArtisanId/{artisanId}")]
        [Authorize(Policy = "ArtisanPolicy")]
        public async Task<IActionResult> GetProductsByArtisanId(Guid artisanId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] ProductStatusEnum? productStatus = null)
        {
            if (artisanId == Guid.Empty)
            {
                return BadRequest(new { Error = 1, Message = "Mã nghệ nhân không hợp lệ." });
            }
            var result = await _productService.GetProductsByArtisanId(artisanId, pageIndex, pageSize, productStatus);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetProductsByStatus")]
        public async Task<IActionResult> GetProductsByStatus([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] ProductStatusEnum productStatus = ProductStatusEnum.Active)
        {
            var result = await _productService.GetProductsByStatus(pageIndex, pageSize, productStatus);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            if (request == null || request.Images == null)
            {
                return BadRequest(new { Error = 1, Message = "Dữ liệu sản phẩm không hợp lệ." });
            }
            var result = await _productService.CreateProduct(request);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch("UpdateProduct")]
        [Authorize(Policy = "ArtisanPolicy")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            var result = await _productService.UpdateProduct(request);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Policy = "ArtisanPolicy")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { Error = 1, Message = "Mã sản phẩm không hợp lệ." });
            }
            var result = await _productService.DeleteProduct(id);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
