using CGP.Contract.DTO.Transaction;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface ITransactionService 
    {
        Task<Result<List<ViewTransactionDTO>>> GetAllTransactionsAsync();
        Task<Result<ViewTransactionDTO>> GetTransactionByIdAsync(Guid transactionId);
        Task<Result<List<ViewTransactionDTO>>> GetAllTransactionsByUserIdAsync(Guid userId);
    }
}
