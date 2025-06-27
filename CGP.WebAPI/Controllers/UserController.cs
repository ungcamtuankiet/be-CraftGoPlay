using CGP.Application.Interfaces;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-current-user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var getUser = await _userService.GetCurrentUserById();
            return Ok(getUser);
        }

    }
}
