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
        Task<Result<object>> EscalateReturnRequestAsync(Guid returnRequestId, string reason);
        Task<Result<object>> GetReturnRequestByUserIdAsync(Guid userId, ReturnStatusEnum? status, int pageIndex, int pageSize);
        Task<Result<object>> GetReturnRequestByArtisanIdAsync(Guid artisanId, ReturnStatusEnum? status, int pageIndex, int pageSize);
        Task<Result<object>> GetEscalatedReturnRequestAsync(int pageIndex, int pageSize);
        Task<Result<object>> UpdateStatusReturnRequestAsync(Guid returnRequestId, ReturnStatusEnum status, RejectReturnReasonEnum rejectReason);
        Task<Result<object>> ResolveEscalatedRequestAsync(Guid returnRequestId, bool acceptRefund);
    }
}
