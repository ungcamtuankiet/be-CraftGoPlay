using CGP.Application.Interfaces;
using CGP.Contract.DTO.ArtisanRequest;
using CGP.Contract.DTO.User;
using CGP.Contract.DTO.UserAddress;
using CGP.Domain.Enums;
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

        [HttpGet("CheckRequestSent/{userId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CheckAlreadySentRequest(Guid userId)
        {
            var existing = await _artisanRequestService.GetPendingRequestByUserId(userId);
            if (existing != null)
            {
                return Ok(new
                {
                    isSent = true,
                    message = "Bạn đã gửi yêu cầu và đang chờ duyệt."
                });
            }

            return Ok(new
            {
                isSent = false,
                message = "Bạn chưa gửi yêu cầu."
            });
        }

        [HttpGet("GetSentRequestByUserId/{userId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetSentRequestByUserId(Guid userId)
        {
            var result = await _artisanRequestService.GetLatestRequestByUserId(userId);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        // ======== POST ========
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

        // ======== PUT ========
        [HttpPut("UpdateInfoUser")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateUserInfo([FromForm] UpdateInfoUserDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var result = await _userService.UpdateUserInfoAsync(updateDto);

            if (result.Error != 0)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("CancelRequest/{userId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CancelRequest(Guid userId)
        {
            var result = await _artisanRequestService.CancelRequestByArtisan(userId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("ResendRequest")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> ResendRequest([FromQuery] Guid userId, [FromQuery] Guid requestId)
        {
            var result = await _artisanRequestService.ResendRequest(userId, requestId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
