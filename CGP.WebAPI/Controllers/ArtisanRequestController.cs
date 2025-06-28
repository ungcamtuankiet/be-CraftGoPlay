using CGP.Application.Interfaces;
using CGP.Contract.DTO.ArtisanRequest;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtisanRequestController : ControllerBase
    {
        private readonly IArtisanRequestService _artisanRequestService;

        public ArtisanRequestController(IArtisanRequestService artisanRequestService)
        {
            _artisanRequestService = artisanRequestService;
        }

        [HttpGet("GetAllRequest")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllRequest()
        {
            var result = await _artisanRequestService.GetAllRequestsAsync();
            if (result.Error != 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("GetRequestById/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetRequestById(Guid id)
        {
            var result = await _artisanRequestService.GetRequestByIdAsync(id);
            if (result.Error != 0)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("GetRequestByStatus")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetRequestByStatus([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] RequestArtisanStatus status = RequestArtisanStatus.Pending)
        {
            var result = await _artisanRequestService.GetRequestByStatus(pageIndex, pageSize, status);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("ApprovedRequest/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ApprovedRequest(Guid id)
        {
            var result = await _artisanRequestService.ApprovedRequest(id);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("RejectedRequest")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> RejectedRequest(RejectRequestDTO reject)
        {
            var result = await _artisanRequestService.RejectedRequest(reject);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
