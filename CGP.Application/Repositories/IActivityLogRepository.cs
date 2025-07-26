using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IActivityLogRepository : IGenericRepository<ActivityLog>
    {
        Task<List<ActivityLog>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize);
    }
}
