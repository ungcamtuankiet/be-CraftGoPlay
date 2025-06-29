﻿using CGP.Application.Interfaces;
using CGP.Contract.DTO.ArtisanRequest;
using CGP.Contract.DTO.UserAddress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IArtisanRequestService _artisanRequestService;

        public UserController(IUserService userService, IArtisanRequestService artisanRequestService)
        {
            _userService = userService;
            _artisanRequestService = artisanRequestService;
        }

        [HttpGet("get-current-user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var getUser = await _userService.GetCurrentUserById();
            return Ok(getUser);
        }

        [HttpPost("SendRequestUpgradeToArtisan")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> SendRequestUpgradeToArtisan([FromForm] SendRequestDTO request)
        {
            var result = await _artisanRequestService.SendRequestAsync(request);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("CancelRequest/{id}")]
        [Authorize(Policy = "ArtisanPolicy")]
        public async Task<IActionResult> CancelRequest(Guid id)
        {
            var result = await _artisanRequestService.CancelRequestByArtisan(id);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
