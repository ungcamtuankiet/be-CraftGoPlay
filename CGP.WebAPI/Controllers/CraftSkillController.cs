using CGP.Application.Interfaces;
using CGP.Contract.DTO.CraftSkill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CraftSkillController : ControllerBase
    {
        private readonly ICraftSkillService _craftSkillService;

        public CraftSkillController(ICraftSkillService craftSkillService)
        {
            _craftSkillService = craftSkillService;
        }

        [HttpGet("GetAllCraftSkills")]
        public async Task<IActionResult> GetAllCraftSkills()
        {
            var result = await _craftSkillService.GetAllCraftSkillsAsync();
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetCraftSkillById/{id}")]
        public async Task<IActionResult> GetCraftSkillById(Guid id)
        {
            var result = await _craftSkillService.GetCraftSkillByIdAsync(id);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPost("CreateNewCraftSkill")]
        [Authorize(Policy = "AdminOrStaffPolicy")]
        public async Task<IActionResult> CreateNewCraftSkill([FromBody] CreateCraftSkillDTO createCraftSkillDTO)
        {
            var result = await _craftSkillService.CreateNewCraftSkill(createCraftSkillDTO);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("UpdateCraftSkill")]
        [Authorize(Policy = "AdminOrStaffPolicy")]
        public async Task<IActionResult> UpdateCraftSkill([FromBody] UpdateCraftSkillDTO updateCraftSkillDTO)
        {
            var result = await _craftSkillService.UpdateNewCraftSkill(updateCraftSkillDTO);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("DeleteCraftSkill/{id}")]
        [Authorize(Policy = "AdminOrStaffPolicy")]
        public async Task<IActionResult> DeleteCraftSkill(Guid id)
        {
            var result = await _craftSkillService.DeleteCraftSkill(id);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
