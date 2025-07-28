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
                .FirstOrDefaultAsync(w => w.User_Id == userId && w.Type == WalletTypeEnum.User);
        }
        public async Task<Wallet> GetWalletByArtisanIdAsync(Guid artianId)
        {
            return await _context.Wallet
                .Include(w => w.WalletTransactions)
                .FirstOrDefaultAsync(w => w.User_Id == artianId && w.Type == WalletTypeEnum.Artisan);
        }

        public async Task<Wallet> GetWalletSystem()
        {
            return await _context.Wallet
                .Include(w => w.WalletTransactions)
                .FirstOrDefaultAsync(w =>w.Type == WalletTypeEnum.System);
        }
    }
}
