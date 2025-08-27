using CGP.Application.Interfaces;
using CGP.Contract.DTO.DashBoard;
using CGP.Contracts.Abstractions.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public DashboardController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("DashboardForArtisan")]
        public async Task<IActionResult> GetArtisanDashboard([FromQuery] RevenueFilterDto filterDto)
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
            var dashboard = await _orderService.GetDashboardAsync(filterDto);
            return Ok(dashboard);
        }
    }
}
