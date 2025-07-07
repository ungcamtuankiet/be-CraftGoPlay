using CGP.Application.Interfaces;
using CGP.Contract.DTO.User;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllAccountByStatus")]
        [Authorize(Policy = "AdminOrStaffPolicy")]
        public async Task<IActionResult> GetAllAccountByStatus([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] StatusEnum status = StatusEnum.Active)
        {
            var result = await _userService.GetAllAccountByStatusAsync(pageIndex, pageSize, status);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("CreateNewAccount")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateNewAccount([FromForm] CreateNewAccountDTO createNewAccountDTO)
        {
            var result = await _userService.CreateNewAccountAsync(createNewAccountDTO);
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = errors
                });
            }
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
