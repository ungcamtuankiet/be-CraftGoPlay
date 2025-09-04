using CGP.Contract.DTO.Wallet;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IPayoutService
    {
        Task<string> CreatePaymentUrl(Guid transactionId, decimal totalAmount, HttpContext context);
        Task<Result<string>> CreateWithdrawUrl(WithDraw withDraw);
        Task<bool> ValidateReturnData(IQueryCollection query);
        Task<Result<bool>> RefundAsync(Guid orderId);
        Task<string> QueryTransactionAsync(string txnRef, string orderInfo, string transactionDate);
        Task<Transaction> HandleWithdrawReturn(Guid transactionId, TransactionStatusEnum status);
    }
}
