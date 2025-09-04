using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IUserVoucherService
    {
        public Task<Result<object>> GetAllVouchersByUserId(Guid userId, VoucherTypeEnum? voucherType);
        public Task<Result<object>> SwapVoucher(Guid userId, Guid voucherId);
    }
}
