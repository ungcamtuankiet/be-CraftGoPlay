using CGP.Application.Interfaces;
using CGP.Contract.DTO.UserQuest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserQuestController : ControllerBase
    {
        private readonly IUserQuestService _userQuestService;
        public UserQuestController(IUserQuestService userQuestService)
        {
            _userQuestService = userQuestService;
        }

        [HttpGet("GetUserQuests")]
        public async Task<IActionResult> GetUserQuests([FromQuery] Guid userId)
        {
            var result = await _userQuestService.GetUserQuestsAsync(userId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("CompleteQuest")]
        public async Task<IActionResult> CompleteQuest([FromQuery] ClaimRewardDTO claim)
        {
            var result = await _userQuestService.ClaimDailyRewardAsync(claim.UserId, claim.UserQuestId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
