using CGP.Application.Interfaces;
using CGP.Application.Services;
using CGP.Contract.DTO.RefundRequest;
using CGP.Contracts.Abstractions.Shared;
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

        [HttpPost("ReturnRequest")]
        [Authorize(Policy = "UserPolicy")]
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
    }
}
