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
            var dashboard = await _orderService.GetDashboardForArtisan(filterDto);
            return Ok(dashboard);
        }

        [HttpGet("DashboardForAdmin")]
        public async Task<IActionResult> GetAdminDashboard([FromQuery] RevenueFilterForAdmin filterDto)
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
            var dashboard = await _orderService.GetDashboardForAdmin(filterDto);
            return Ok(dashboard);
        }

        [HttpGet("ProductCountsByMonth")]
        public async Task<IActionResult> GetProductCountsByMonth([FromQuery] int year, [FromQuery] Guid? artisanId = null)
        {
            if (year < 1900 || year > DateTime.UtcNow.Year)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Năm không hợp lệ."
                });
            }

            var result = await _orderService.GetProductCountsByMonthAsync(year, artisanId);
            return Ok(result);
        }
    }
}
