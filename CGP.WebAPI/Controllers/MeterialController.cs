using CGP.Application.Interfaces;
using CGP.Contract.DTO.Meterial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterialController : ControllerBase
    {
        private readonly IMeterialService _meterialService;

        public MeterialController(IMeterialService meterialService)
        {
            _meterialService = meterialService;
        }

        [HttpGet("GetMeterials")]
        public async Task<IActionResult> GetMeterials()
        {
            var result = await _meterialService.GetMeterialsAsync();
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("CreateMeterial")]
        [Authorize(Policy = "AdminOrStaffPolicy")]
        public async Task<IActionResult> CreateMeterial([FromBody] MeterialCreateDTO request)
        {
            if (request == null)
            {
                return BadRequest("Dữ liệu yêu cầu không hợp lệ.");
            }
            var result = await _meterialService.CreateMeterial(request);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("UpdateMeterial")]
        [Authorize(Policy = "AdminOrStaffPolicy")]
        public async Task<IActionResult> UpdateMeterial([FromBody] MeterialUpdateDTO request)
        {
            if (request == null || request.Id == Guid.Empty)
            {
                return BadRequest("Dữ liệu yêu cầu không hợp lệ.");
            }
            var result = await _meterialService.UpdateMeterial(request);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("DeleteMeterial/{id}")]
        [Authorize(Policy = "AdminOrStaffPolicy")]
        public async Task<IActionResult> DeleteMeterial(Guid id)
        {
            var result = await _meterialService.DeleteMeterial(id);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
