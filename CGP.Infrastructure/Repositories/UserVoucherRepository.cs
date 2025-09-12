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
    public class UserVoucherRepository : GenericRepository<UserVoucher>, IUserVoucherRepository
    {
        private readonly AppDbContext _context;
        public UserVoucherRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<bool> CheckExistUserVoucher(Guid userId, Guid voucherId)
        {
            var userHasVoucher = await _context.UserVouchers
                .FirstOrDefaultAsync(u => u.Id == userId && u.VoucherId == voucherId);

            if(userHasVoucher != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<UserVoucher>> GetAllsVoucherByUserId(Guid userId, VoucherTypeEnum? voucherTypeEnum)
        {
            var query = _context.UserVouchers
                .Where(u => u.UserId == userId && u.IsUsed == false)
                .AsQueryable();

            if (voucherTypeEnum != null)
            {
                query = query.Where(u => u.Voucher.Type == voucherTypeEnum)
                             .Include(u => u.Voucher);
            }
            else
            {
                query = query.Include(u => u.Voucher);
            }

            return await query.ToListAsync();
        }

        public async Task<UserVoucher> GetUserVoucher(Guid userId, Guid voucherId)
        {
            return await _context.UserVouchers
                .FirstOrDefaultAsync(uv => uv.UserId == userId && uv.VoucherId == voucherId);
        }
    }
}
