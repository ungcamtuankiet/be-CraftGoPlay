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

        [HttpPost("AddCrop")]
        public async Task<IActionResult> AddCrop([FromForm] AddCropDTO cropDto)
        {
            if (cropDto == null)
            {
                return BadRequest("Crop data is required.");
            }
            var result = await _cropService.AddCropAsync(cropDto);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateCrop")]
        public async Task<IActionResult> UpdateCrop([FromForm] UpdateCropDTO cropDto)
        {
            if (cropDto == null || cropDto.Id == Guid.Empty)
            {
                return BadRequest("Valid crop data is required.");
            }
            var result = await _cropService.UpdateCropAsync(cropDto);
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
