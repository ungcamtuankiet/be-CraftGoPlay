using CGP.Application.Interfaces;
using CGP.Contract.DTO.Voucher;
using CGP.Contracts.Abstractions.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet("GetAllVouchers")]
        public async Task<IActionResult> GetAllVouchers()
        {
            var result = await _voucherService.GetAllVouchersAsync();
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetVoucherById/{voucherId}")]
        public async Task<IActionResult> GetVoucherById(Guid voucherId)
        {
            var result = await _voucherService.GetVoucherByIdAsync(voucherId);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("GetAllVouchersByCode/{voucherCode}")]
        public async Task<IActionResult> GetAllVouchersByCode(string voucherCode)
        {
            var result = await _voucherService.GetAllVouchersByCodeAsync(voucherCode);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("CreateVoucher")]
        public async Task<IActionResult> CreateVoucher([FromForm] CreateVoucherDTO dto)
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
            var result = await _voucherService.CreateVoucherAsync(dto);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateVoucher")]
        public async Task<IActionResult> UpdateVoucher([FromForm] UpdateVoucherDTO dto)
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
            var result = await _voucherService.UpdateVoucherAsync(dto);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
