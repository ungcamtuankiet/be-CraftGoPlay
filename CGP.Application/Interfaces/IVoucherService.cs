using CGP.Contract.DTO.Voucher;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IVoucherService 
    {
        Task<Result<List<ViewVoucherDTO>>> GetAllVouchersAsync();
        Task<Result<ViewVoucherDTO>> GetVoucherByIdAsync(Guid voucherId);
        Task<Result<ViewVoucherDTO>> GetAllVouchersByCodeAsync(string voucherCode);
        Task<Result<object>> CreateVoucherAsync(CreateVoucherDTO dto);
        Task<Result<object>> UpdateVoucherAsync(UpdateVoucherDTO dto);
        Task<Result<object>> RemoveVoucherAsync(Guid voucherId);
    }
}
