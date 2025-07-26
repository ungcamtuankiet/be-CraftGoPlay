using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Repositories
{
    public class ActivityLogRepository : GenericRepository<ActivityLog> ,IActivityLogRepository
    {
        private readonly AppDbContext _dbContext;

        public ActivityLogRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ActivityLog>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize)
        {
            return await _dbContext.ActivityLog
                .Where(log => log.UserId == userId && !log.IsDeleted)
                .OrderByDescending(log => log.CreationDate)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
