using CGP.Contract.DTO.RefundRequest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Enums;
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
        Task<Result<object>> GetReturnRequestByArtisanIdAsync(Guid artisanId, ReturnStatusEnum? status, int pageIndex, int pageSize);
        Task<Result<object>> UpdateStatusReturnRequestAsync(Guid returnRequestId, ReturnStatusEnum status);
    }
}
