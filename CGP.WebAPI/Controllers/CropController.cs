using CGP.Application.Interfaces;
using CGP.Contract.DTO.Crop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CropController : ControllerBase
    {
        private readonly ICropService _cropService;
        public CropController(ICropService cropService)
        {
            _cropService = cropService;
        }

        [HttpGet("GetCropById/{id}")]
        public async Task<IActionResult> GetCropById(Guid id)
        {
            var result = await _cropService.GetCropByIdAsync(id);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("GetCropsByUserId/{userId}")]
        public async Task<IActionResult> GetCropsByUserId(Guid userId)
        {
            var result = await _cropService.GetCropsByUserIdAsync(userId);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("PlantCrop")]
        public async Task<IActionResult> PlantCrop([FromBody] PlantCropDTO request)
        {
            var result = await _cropService.PlantCropAsync(request);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateCrop")]
        public async Task<IActionResult> UpdateCrop([FromBody] UpdateCropDTO request)
        {
            var result = await _cropService.UpdateCropCropAsync(request);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("WaterCrop/{cropId}")]
        public async Task<IActionResult> WaterCrop(Guid cropId)
        {
            var result = await _cropService.WaterCropAsync(cropId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("HarvestCrop/{cropId}")]
        public async Task<IActionResult> HarvestCrop(Guid cropId)
        {
            var result = await _cropService.HarvestCropAsync(cropId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("RemoveCrop/{cropId}")]
        public async Task<IActionResult> RemoveCrop(Guid cropId)
        {
            var result = await _cropService.RemoveCropAsync(cropId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
