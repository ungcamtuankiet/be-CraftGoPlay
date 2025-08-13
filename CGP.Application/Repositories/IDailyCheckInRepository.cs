using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IDailyCheckInRepository : IGenericRepository<DailyCheckIn>
    {
        public Task<DailyCheckIn> IsCheckIn(Guid userId);
    }
}
