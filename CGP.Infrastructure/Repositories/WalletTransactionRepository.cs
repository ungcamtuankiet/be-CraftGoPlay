using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Repositories
{
    public class WalletTransactionRepository : GenericRepository<WalletTransaction>, IWalletTransactionRepository
    {
        private readonly AppDbContext _context;

        public WalletTransactionRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<List<WalletTransaction>> GetPendingTransactionsAsync(DateTime now)
        {
            return await _context.WalletTransaction
                 .Where(t => t.Type == WalletTransactionTypeEnum.Pending && t.UnlockDate <= now)
                .Include(t => t.Wallet)
                .OrderByDescending(t => t.ModificationDate)
                .ToListAsync();
        }
    }
}
