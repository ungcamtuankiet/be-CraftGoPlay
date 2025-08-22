using CGP.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmlandController : ControllerBase
    {
        private readonly IFarmlandService _farmlandService;

        public FarmlandController(IFarmlandService farmlandService)
        {
            _farmlandService = farmlandService;
        }

        [HttpGet("GetFarmlands/{userId}")]
        public async Task<IActionResult> GetFarmlandsByUserId(Guid userId)
        {
            var result = await _farmlandService.GetFarmlandsByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpPost("Dig/{plotId}")]
        public async Task<IActionResult> DigAsync(Guid plotId)
        {
            var result = await _farmlandService.DigAsync(plotId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("Plant/{plotId}/{cropId}")]
        public async Task<IActionResult> PlantAsync(Guid plotId, Guid cropId)
        {
            var result = await _farmlandService.PlantAsync(plotId, cropId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Reset/{plotId}")]
        public async Task<IActionResult> ResetAsync(Guid plotId)
        {
            var result = await _farmlandService.ResetAsync(plotId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
