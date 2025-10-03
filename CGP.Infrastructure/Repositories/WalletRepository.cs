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

namespace CGP.Infrastructure.Repositories {
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {

        private readonly AppDbContext _context;
        public WalletRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<Wallet> GetWalletByUserIdAsync(Guid userId)
        {
            return await _context.Wallet
                .Include(w => w.WalletTransactions)
                .Where(w => w.User_Id == userId && w.Type == WalletTypeEnum.User)
                .Select(w => new Wallet
                {
                    Id = w.Id,
                    User_Id = w.User_Id,
                    Type = w.Type,
                    PendingBalance = w.PendingBalance,
                    AvailableBalance = w.AvailableBalance,
                    WalletTransactions = w.WalletTransactions
                        .OrderByDescending(t => t.CreationDate)
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<Wallet> GetWalletByArtisanIdAsync(Guid artianId)
        {
            var wallet = await _context.Wallet
                .Include(w => w.WalletTransactions)
                .FirstOrDefaultAsync(w => w.User_Id == artianId && w.Type == WalletTypeEnum.Artisan);

            if (wallet != null)
            {
                wallet.WalletTransactions = wallet.WalletTransactions
                    .OrderByDescending(t => t.CreationDate) // đổi CreationDate thành field ngày của bạn
                    .ToList();
            }

            return wallet;
        }
        public async Task<Wallet> GetWalletSystem()
        {
            var wallet = await _context.Wallet
                .Include(w => w.WalletTransactions)
                .FirstOrDefaultAsync(w => w.Type == WalletTypeEnum.System);

            if (wallet != null)
            {
                wallet.WalletTransactions = wallet.WalletTransactions
                    .OrderByDescending(t => t.CreationDate)
                    .ToList();
            }

            return wallet;
        }
    }
}
