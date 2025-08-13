using CGP.Application.Interfaces;
using CGP.Contract.DTO.CartItem;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("GetAllItemCart/{userId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetCart(Guid userId)
        {
            var result = await _cartService.ViewCartAsync(userId);
            if (result.Error != 0) return NotFound(result);
            return Ok(result);
        }

        [HttpPost("AddToCart/{userId}")]
/*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> AddToCart(Guid userId, [FromBody] AddCartItemDto dto)
        {
            var result = await _cartService.AddToCartAsync(userId, dto);
            if (result.Error != 0)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("UpdateCart")]
/*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto)
        {
            var result = await _cartService.UpdateCartItemAsync(dto);
            if (result.Error != 0)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("Delete/{cartItemId}")]
/*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
        {
            var result = await _cartService.RemoveFromCartAsync(cartItemId);
            if (result.Error != 0)
                return NotFound(result);

            return Ok(result);
        }
    }
}
