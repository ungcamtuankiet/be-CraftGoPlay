using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IActivityLogService
    {
        public Task<List<Result<object>>> ViewActivityLogByUserId(Guid userid, int pageIndex, int pageSize);
    }
}
