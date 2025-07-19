using CGP.Application.Interfaces;
using CGP.Contract.DTO.Order;
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

        [HttpPost("create-from-cart")]
        public async Task<IActionResult> CreateFromCart([FromForm] CreateOrderFromCartDto request)
        {
            var result = await _orderService.CreateOrderFromCartAsync(request.UserId, request.SelectedCartItemIds,request.AddressId, request.PaymentMethod);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpPost("create-direct")]
        public async Task<IActionResult> CreateDirect([FromForm] CreateDirectOrderDto dto, Guid userId)
        {
            var result = await _orderService.CreateDirectOrderAsync(userId, dto);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpGet("vnpay-url/{orderId}")]
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
    }
}
