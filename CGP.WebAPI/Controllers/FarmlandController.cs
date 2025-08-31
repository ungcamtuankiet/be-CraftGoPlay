using CGP.Application.Interfaces;
using CGP.Contract.DTO.Farmland;
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

        [HttpPost("Plow")]
        public async Task<IActionResult> PlowAsync([FromForm] PlowCropDTO plow)
        {
            var result = await _farmlandService.PlowAsync(plow);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPut("Plant")]
        public async Task<IActionResult> PlantAsync([FromForm] PlantCropDTO plant)
        {
            var result = await _farmlandService.PlantCropAsync(plant);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("Water")]
        public async Task<IActionResult> WaterAsync([FromForm] WateredCropDTO water)
        {
            var result = await _farmlandService.WaterAsync(water);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Harvest")]
        public async Task<IActionResult> HarvestAsync([FromForm] HavestCropDTO havest)
        {
            var result = await _farmlandService.HarvestAsync(havest);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
