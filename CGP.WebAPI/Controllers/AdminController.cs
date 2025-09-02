using CGP.Application.Interfaces;
using CGP.Application.Services;
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
        private readonly IOrderService _orderService;

        public AdminController(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        [HttpGet("GetAllAccount")]
        //[Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllAccount([FromQuery]int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] StatusEnum? status = null, [FromQuery] RoleEnum? role = null)
        {
            var result = await _userService.GetAllAccount(pageIndex, pageSize, status, role);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("CountAllOrders")]
        //[Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CountAllOrders()
        {
            var result = await _orderService.CountAllOrdersAsync();
            return StatusCode(result.Error == 0 ? 200 : 400, result);
        }

        [HttpGet("CountAccountByRole")]
        //[Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CountUsersByRole()
        {
            var result = await _userService.GetUserCountByRoleAsync();
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("CreateStaffAccount")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateStaffAccount([FromForm] CreateNewAccountDTO createNewAccountDTO)
        {
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
            var result = await _userService.CreateNewAccountAsync(createNewAccountDTO);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut("UpdateAccount")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateAccount([FromForm] UpdateAccountDTO updateAccountDTO)
        {
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
            var result = await _userService.UpdateAccountAsync(updateAccountDTO);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("DeleteAccount/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var result = await _userService.DeleteAccountAsync(id);
            if (result.Error == 0)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }
    }
}
