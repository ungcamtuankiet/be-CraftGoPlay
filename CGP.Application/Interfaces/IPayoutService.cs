using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
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
        Task<bool> ValidateReturnData(IQueryCollection query);
        Task<Result<bool>> RefundAsync(Guid orderId);
    }
}
