using CGP.Application.Interfaces;
using CGP.Contract.DTO.Wallet;
using CGP.Domain.Enums;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VNPAY.NET.Enums;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IPayoutService _payoutService;

        public WalletController(IWalletService walletService, IPayoutService payoutService)
        {
            _walletService = walletService;
            _payoutService = payoutService;
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

        [HttpPost("Withdraw")]
        public async Task<IActionResult> WithDraw([FromForm] WithDraw withDraw)
        {
            var url = await _payoutService.CreateWithdrawUrl(withDraw);
            return Ok(new { WithdrawUrl = url });
        }

        [HttpGet("withdraw-return")]
        public async Task<IActionResult> WithdrawReturn(Guid transactionId, TransactionStatusEnum status)
        {
            var transaction = await _payoutService.HandleWithdrawReturn(transactionId, status);
            if (transaction == null) return NotFound();

            return Ok(transaction);
        }
    }
}
