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

        [HttpGet("HasCheckedIn/{userId}")]
        public async Task<IActionResult> HasCheckedInToday(Guid userId)
        {
            var result = await _dailyCheckInService.HasCheckedInTodayAsync(userId);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("CurrentStreak/{userId}")]
        public async Task<IActionResult> GetCurrentStreak(Guid userId)
        {
            var result = await _dailyCheckInService.GetCurrentStreakAsync(userId);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("CheckIn")]
        public async Task<IActionResult> CheckIn([FromForm] DailyCheckInDTO dto)
        {
            var result = await _dailyCheckInService.CheckInAsync(dto);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("UpdateStreak/{userId}")]
        public async Task<IActionResult> UpdateStreak(Guid userId)
        {
            var result = await _dailyCheckInService.UpdateStreakAsync(userId);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
