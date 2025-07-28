using CGP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("GetWalletByUserId/{userId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetWalletByUserId(Guid userId)
        {
            var result = await _walletService.GetWalletByUserId(userId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetWalletByArtisanId/{artisanId}")]
        [Authorize(Policy = "ArtisanPolicy")]
        public async Task<IActionResult> GetWalletByArtisanId(Guid artisanId)
        {
            var result = await _walletService.GetWalletByArtisanId(artisanId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetWalletSystem")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetWalletSystem()
        {
            var result = await _walletService.GetWalletSystem();
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
