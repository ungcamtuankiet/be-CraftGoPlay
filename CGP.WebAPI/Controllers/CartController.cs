using CGP.Application.Interfaces;
using CGP.Contract.DTO.CartItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(Guid userId)
        {
            var result = await _cartService.ViewCartAsync(userId);
            if (result.Error != 0) return NotFound(result.Message);
            return Ok(result);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddToCart(Guid userId, [FromBody] AddCartItemDto dto)
        {
            var result = await _cartService.AddToCartAsync(userId, dto);
            if (result.Error != 0)
                return BadRequest(new { result.Error, result.Message });

            return Ok(result);
        }

        [HttpPut("item")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto)
        {
            var result = await _cartService.UpdateCartItemAsync(dto);
            if (result.Error != 0)
                return NotFound(new { result.Error, result.Message });

            return Ok(result);
        }

        [HttpDelete("item/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
        {
            var result = await _cartService.RemoveFromCartAsync(cartItemId);
            if (result.Error != 0)
                return NotFound(new { result.Error, result.Message });

            return Ok(result);
        }
    }
}
