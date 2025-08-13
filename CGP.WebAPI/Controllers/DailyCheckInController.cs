using CGP.Application.Interfaces;
using CGP.Contract.DTO.DailyCheckIn;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyCheckInController : ControllerBase
    {
        private readonly IDailyCheckInService _dailyCheckInService;
        public DailyCheckInController(IDailyCheckInService dailyCheckInService)
        {
            _dailyCheckInService = dailyCheckInService;
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn([FromForm] DailyCheckInDTO dto)
        {
            var result = await _dailyCheckInService.CheckInAsync(dto);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
