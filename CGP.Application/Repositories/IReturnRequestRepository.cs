using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IReturnRequestRepository : IGenericRepository<ReturnRequest>
    {
        Task<List<ReturnRequest>> GetAllAsync(int pageIndex, int pageSize, ReturnStatusEnum status);
        Task<List<ReturnRequest>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize, ReturnStatusEnum status);
        Task<List<ReturnRequest>> GetByArtisanIdAsync(Guid artisanId, int pageIndex, int pageSize, ReturnStatusEnum? status);
        Task<ReturnRequest> GetByOrderItemIdAsync(Guid orderItemId);
        Task<ReturnRequest> GetReturnRequestByOrderItemIdAsync(Guid orderItemId);
    }
}
