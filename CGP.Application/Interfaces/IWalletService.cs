using CGP.Contract.DTO.Wallet;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IWalletService
    {
        Task<Result<ViewWalletDTO>> GetWalletByUserId(Guid userId);
        Task<Result<ViewWalletDTO>> GetWalletByArtisanId(Guid artisanId);
        Task<Result<ViewWalletDTO>> GetWalletSystem();
        Task CreatePendingTransactionAsync(Guid shopId, decimal amount, int holdDays);
        Task ReleasePendingTransactionsAsync();
    }
}
