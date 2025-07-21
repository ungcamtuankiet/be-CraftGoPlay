using CGP.Application.Interfaces;
using CGP.Contract.DTO.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetOrdersByUserId/{userId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetOrdersByUserId(Guid userId)
        {
            var result = await _orderService.GetOrdersByUserIdAsync(userId);
            if (result.Error == 1)
            {
                return NotFound(result);
            }
            if (result.Error == 2)
            {
                return Forbid();
            }
            return Ok(result);
        }

        [HttpGet("GetOrdersByArtisanId/{artisanId}")]
        [Authorize(Policy = "ArtisanPolicy")] // Assuming artisan policy exists
        public async Task<IActionResult> GetOrdersByArtisanId(Guid artisanId)
        {
            var result = await _orderService.GetOrdersByArtisanIdAsync(artisanId);
            if (result.Error == 1)
            {
                return NotFound(result);
            }
            if (result.Error == 2)
            {
                return Forbid();
            }
            return Ok(result);
        }

        [HttpGet("GetOrderByOrderId/{orderId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetOrderByOrderId(Guid orderId)
        {
            var result = await _orderService.GetOrderByIdAssync(orderId);
            if (result.Error == 1)
            {
                return NotFound(result);
            }
            if (result.Error == 2)
            {
                return Forbid();
            }
            return Ok(result);
        }

        [HttpGet("VnpayUrl/{orderId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CreateVnPayUrl(Guid orderId)
        {
            var result = await _orderService.CreateVnPayUrlAsync(orderId, HttpContext);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            var result = await _orderService.HandleVnPayReturnAsync(Request.Query);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpPost("CreateFromCart")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CreateFromCart([FromForm] CreateOrderFromCartDto request)
        {
            var result = await _orderService.CreateOrderFromCartAsync(request.UserId, request.SelectedCartItemIds,request.AddressId, request.PaymentMethod);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpPost("CreateDirect/{userId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CreateDirect([FromForm] CreateDirectOrderDto dto, Guid userId)
        {
            var result = await _orderService.CreateDirectOrderAsync(userId,dto.AddressId, dto);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpPut("status/{orderId}")]
        [Authorize(Policy = "ArtisanPolicy")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromForm] UpdateOrderStatusDto statusDto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, statusDto);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }
    }
}
