﻿using CGP.Application.Interfaces;
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

        [HttpGet("GetAllAccountByStatus")]
        [Authorize(Policy = "AdminOrStaffPolicy")]
        public async Task<IActionResult> GetAllAccountByStatus([FromQuery]int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] StatusEnum status = StatusEnum.Active)
        {
            var result = await _userService.GetAllAccountByStatusAsync(pageIndex, pageSize, status);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("GetOrders")]
        [Authorize(Policy = "AdminOrStaffPolicy")] // Assuming admin access for all orders
        public async Task<IActionResult> GetOrders([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] OrderStatusEnum? status = null)
        {
            var result = await _orderService.GetOrdersAsync(pageIndex, pageSize, status);
            return StatusCode(result.Error == 0 ? 200 : 400, result);
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
