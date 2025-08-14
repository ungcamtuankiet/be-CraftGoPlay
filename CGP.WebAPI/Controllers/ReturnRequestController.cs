using CGP.Application.Interfaces;
using CGP.Application.Services;
using CGP.Contract.DTO.RefundRequest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnRequestController : ControllerBase
    {
        private readonly IReturnRequestService _returnRequestService;

        public ReturnRequestController(IReturnRequestService returnRequestService)
        {
            _returnRequestService = returnRequestService;
        }

        [HttpGet("GetReturnRequestByArtisanId/{artisanId}")]
        public async Task<IActionResult> GetReturnRequestByArtisanId(Guid artisanId,[FromQuery] ReturnStatusEnum? status = null, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _returnRequestService.GetReturnRequestByArtisanIdAsync(artisanId, status, pageIndex, pageSize);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPost("ReturnRequest")]
        /*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> RefundOrder([FromForm] SendRefundRequestDTO dto)
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
            var result = await _returnRequestService.RefundOrderAsync(dto);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateStatusReturnRequest/{returnRequestId}")]
        public async Task<IActionResult> UpdateStatusReturnRequest(Guid returnRequestId, ReturnStatusEnum status)
        {
            var result = await _returnRequestService.UpdateStatusReturnRequestAsync(returnRequestId, status);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
