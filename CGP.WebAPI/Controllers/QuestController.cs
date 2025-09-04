using CGP.Application.Interfaces;
using CGP.Contract.DTO.Quest;
using CGP.Contracts.Abstractions.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly IQuestService _questService;
        public QuestController(IQuestService questService)
        {
            _questService = questService;
        }

        [HttpGet("GetAllQuests")]
        public async Task<IActionResult> GetAllQuests()
        {
            var result = await _questService.GetAllQuests();
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("GetQuestById/{id}")]
        public async Task<IActionResult> GetQuestById(Guid id)
        {
            var result = await _questService.GetQuestById(id);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("CreateQuest")]
        public async Task<IActionResult> CreateQuest([FromForm] CreateQuestDTO createQuestDTO)
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
            var result = await _questService.CreateQuest(createQuestDTO);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
