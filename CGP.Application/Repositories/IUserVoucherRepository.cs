using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IUserVoucherRepository : IGenericRepository<UserVoucher>
    {
        Task<bool> CheckExistUserVoucher(Guid userId, Guid voucherId);
        Task<List<UserVoucher>> GetAllsVoucherByUserId(Guid userId, VoucherTypeEnum? voucherTypeEnum);
        Task<UserVoucher> GetUserVoucher(Guid userId, Guid voucherId);
    }
}
