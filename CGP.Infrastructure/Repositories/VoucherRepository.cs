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
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        private readonly AppDbContext _context;
        public VoucherRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<bool> CheckVoucherCode(string code)
        {
            var checkVoucherCode = await _context.Voucher.FirstOrDefaultAsync(v => v.Code == code);
            if (checkVoucherCode == null)
                return false;
            return true;
        }

        public async Task<Voucher> CheckVoucherDelivery(string code)
        {
            var result = await _context.Voucher
                .FirstOrDefaultAsync(v => v.Code == code && v.Type == VoucherTypeEnum.Delivery);

            return result;
        }

        public async Task<Voucher> CheckVoucherProduct(string code)
        {
            var result = await _context.Voucher
                .FirstOrDefaultAsync(v => v.Code == code && v.Type == VoucherTypeEnum.Product);

            return result;
        }

        public async Task<IList<Voucher>> GetAllVoucherAsync()
        {
            return await _context.Voucher
                .OrderByDescending(v => v.CreationDate)
                .ToListAsync();
        }

        public async Task<Voucher> GetVoucherByCodeAsync(string code)
        {
            return await _context.Voucher
                .FirstOrDefaultAsync(v => v.Code == code);
        }
    }
}
