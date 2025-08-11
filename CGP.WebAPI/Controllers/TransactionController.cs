using CGP.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("GetAllTransactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var result = await _transactionService.GetAllTransactionsAsync();
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetTransactionById/{transactionId}")]
        public async Task<IActionResult> GetTransactionById(Guid transactionId)
        {
            var result = await _transactionService.GetTransactionByIdAsync(transactionId);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("GetAllTransactionsByUserId/{userId}")]
        public async Task<IActionResult> GetAllTransactionsByUserId(Guid userId)
        {
            var result = await _transactionService.GetAllTransactionsByUserIdAsync(userId);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
