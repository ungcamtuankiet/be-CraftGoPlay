using CGP.Application.Interfaces;
using CGP.Contract.DTO.UserAddress;
using CGP.Contracts.Abstractions.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserAddressController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAddress/{userId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAddress(Guid userId)
        {
            var getAddress = await _userService.GetListAddressByUserId(userId);
            return Ok(getAddress);
        }

        [HttpGet("GetDefaultAddress/{userId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetDefaultAddress(Guid userId)
        {
            var defaultAddress = await _userService.GetDefaultAddressByUserId(userId);
            return Ok(defaultAddress);
        }

        [HttpGet("GetAddressOfArtisan/{artisanId}")]
        public async Task<IActionResult> GetAddressOfArtisan(Guid artisanId)
        {
            var artisanAddress = await _userService.GetAddressOfArtisan(artisanId);
            if (artisanAddress == null)
            {
                return NotFound(new Result<object>
                {
                    Error = 1,
                    Message = "Không tìm thấy địa chỉ của thợ thủ công.",
                    Data = null
                });
            }
            return Ok(artisanAddress);
        }

        [HttpPost("AddNewAddress")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> AddNewAddress([FromForm] AddNewAddressDTO userAddress)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            var result = await _userService.AddNewAddress(userAddress);
            return Ok(result);
        }

        [HttpPatch("UpdateAddress/{addressId}")]
/*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> UpdateAddress(Guid addressId, [FromForm] UpdateAddressDTO userAddress)
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
            var result = await _userService.UpdateAddress(userAddress, addressId);
            return Ok(result);
        }

        [HttpPatch("SetDefaultAddress/{addressId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> SetDefaultAddress(Guid addressId)
        {
            var result = await _userService.SetDefaultAddress(addressId);
            return Ok(result);
        }

        [HttpDelete("DeleteAddress/{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var result = await _userService.DeleteAddress(id);
            return Ok(result);
        }
    }
}
