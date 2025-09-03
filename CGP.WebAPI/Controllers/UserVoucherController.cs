using CGP.Application.Interfaces;
using CGP.Contract.DTO.UserVoucher;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserVoucherController : ControllerBase
    {
        private readonly IUserVoucherService _userVoucherService;

        public UserVoucherController(IUserVoucherService userVoucherService)
        {
            _userVoucherService = userVoucherService;
        }
        [HttpGet("GetAllVouchersByUserId")]
        public async Task<IActionResult> GetAllVouchersByUserId([FromQuery] GetAllVouchersByUserIdDTO getAll)
        {
            var result = await _userVoucherService.GetAllVouchersByUserId(getAll.UserId, getAll.VoucherType);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("SwapVoucher")]
        public async Task<IActionResult> SwapVoucher([FromQuery] SwapVoucherDTO swap)
        {
            var result = await _userVoucherService.SwapVoucher(swap.UserId, swap.VoucherId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
