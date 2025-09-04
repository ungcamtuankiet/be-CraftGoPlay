using CGP.Application.Interfaces;
using CGP.Contract.DTO.Order;
using CGP.Contract.DTO.RefundRequest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
/*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> GetOrdersByUserId(Guid userId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] OrderStatusEnum? status = null)
        {
            var result = await _orderService.GetOrdersByUserIdAsync(userId, pageIndex, pageSize, status);
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
/*        [Authorize(Policy = "ArtisanPolicy")]*/
        public async Task<IActionResult> GetOrdersByArtisanId(Guid artisanId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] OrderStatusEnum? status = null)
        {
            var result = await _orderService.GetOrdersByArtisanIdAsync(artisanId, pageIndex, pageSize, status);
            if (result.Error == 1)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("GetOrderByOrderId/{orderId}")]
/*        [Authorize(Policy = "UserPolicy")]*/
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

        [HttpGet("CountOrdersByArtisanId/{artisanId}")]
        /*        [Authorize(Policy = "ArtisanPolicy")]*/
        public async Task<IActionResult> CountOrdersByArtisanId(Guid artisanId)
        {
            var result = await _orderService.CountOrdersByArtisanIdAsync(artisanId);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpGet("VnpayUrl/{orderId}")]
/*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> CreateVnPayUrl(Guid orderId)
        {
            var result = await _orderService.CreateVnPayUrlAsync(orderId, HttpContext);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            var result = await _orderService.HandleVnPayReturnAsync(Request.Query);
            var status = result.Error == 0 ? "success" : "failed";
            return Redirect($"http://fe-craft-go-play.vercel.app/payment-{status}");
        }

        [HttpPost("CreateFromCart")]
/*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> CreateFromCart([FromForm] CreateOrderFromCartDto request)
        {
            var deliveryAmounts = JsonSerializer.Deserialize<Dictionary<Guid, double>>(request.DeliveryAmounts);
            var result = await _orderService.CreateOrderFromCartAsync(request.UserId, request.SelectedCartItemIds, deliveryAmounts, request.AddressId, request.VoucherDeliveryCode, request.VoucherProductCode, request.PaymentMethod);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpPost("CreateDirect/{userId}")]
/*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> CreateDirect([FromForm] CreateDirectOrderDto dto, Guid userId)
        {
            var result = await _orderService.CreateDirectOrderAsync(userId, dto.AddressId,dto.DeliveryAmount,dto.VoucherDeliveryCode, dto.VoucherProductCode, dto);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpPut("Status/{orderId}")]
        /*        [Authorize]*/
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromForm] OrderStatusEnum statusDto, [FromForm] ReasonDeliveryFailed reason)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, statusDto, reason);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }
    }
}
