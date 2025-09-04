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

        [HttpGet("GetReturnRequestByUserId/{userId}")]
        public async Task<IActionResult> GetReturnRequestByUserId(Guid userId, [FromQuery] ReturnStatusEnum? status = null, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _returnRequestService.GetReturnRequestByUserIdAsync(userId, status, pageIndex, pageSize);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetReturnRequestByArtisanId/{artisanId}")]
        public async Task<IActionResult> GetReturnRequestByArtisanId(Guid artisanId, [FromQuery] ReturnStatusEnum? status = null, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _returnRequestService.GetReturnRequestByArtisanIdAsync(artisanId, status, pageIndex, pageSize);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetListEscalated")]
        public async Task<IActionResult> GetListEscalated(int pageIndex = 1, int pageSize = 10)
        {
            var result = await _returnRequestService.GetEscalatedReturnRequestAsync(pageIndex, pageSize);
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
        public async Task<IActionResult> UpdateStatusReturnRequest(Guid returnRequestId, ReturnStatusEnum status, RejectReturnReasonEnum rejectReturnReasonEnum)
        {
            var result = await _returnRequestService.UpdateStatusReturnRequestAsync(returnRequestId, status, rejectReturnReasonEnum);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("EscalateReturnRequest/{returnRequestId}")]
        public async Task<IActionResult> EscalateReturnRequest(Guid returnRequestId, [FromQuery] string reason)
        {
            var result = await _returnRequestService.EscalateReturnRequestAsync(returnRequestId, reason);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("ResolveEscalatedRequest/{returnRequestId}")]
        public async Task<IActionResult> ResolveEscalatedRequest(Guid returnRequestId, [FromQuery] bool acceptRefund)
        {
            var result = await _returnRequestService.ResolveEscalatedRequestAsync(returnRequestId, acceptRefund);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
