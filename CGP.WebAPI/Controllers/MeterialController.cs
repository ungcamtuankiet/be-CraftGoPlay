using CGP.Application.Interfaces;
using CGP.Contract.DTO.Meterial;
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
        public async Task<IActionResult> CreateMeterial([FromBody] MeterialCreateDTO request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            var result = await _meterialService.CreateMeterial(request);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
