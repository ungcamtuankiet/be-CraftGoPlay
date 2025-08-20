using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IVoucherRepository : IGenericRepository<Voucher>
    {
        public Task<IList<Voucher>> GetAllVoucherAsync();
        public Task<Voucher> CheckVoucherDelivery(string code);
        public Task<Voucher> CheckVoucherProduct(string code);
        public Task<Voucher> GetVoucherByCodeAsync(string code);
        public Task<bool> CheckVoucherCode(string code);
    }
}
