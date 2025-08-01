using CGP.Contract.DTO.RefundRequest;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IReturnRequestService
    {
        Task<Result<bool>> RefundOrderAsync(SendRefundRequestDTO dto);
    }
}
