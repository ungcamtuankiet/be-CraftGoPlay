using CGP.Application.Interfaces;
using CGP.Contract.DTO.UserAddress;
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
        [Authorize]
        public async Task<IActionResult> GetAddress(Guid userId)
        {
            var getAddress = await _userService.GetListAddressByUserId(userId);
            return Ok(getAddress);
        }

        [HttpPost("AddNewAddress")]
        [Authorize]
        public async Task<IActionResult> AddNewAddress([FromBody] AddNewAddressDTO userAddress)
        {
            var result = await _userService.AddNewAddress(userAddress);
            return Ok(result);
        }

        [HttpPatch("UpdateAddress/{addressId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAddress(Guid addressId, [FromBody] UpdateAddressDTO userAddress)
        {
            var result = await _userService.UpdateAddress(userAddress, addressId);
            return Ok(result);
        }

        [HttpDelete("DeleteAddress/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var result = await _userService.DeleteAddress(id);
            return Ok(result);
        }
    }
}
