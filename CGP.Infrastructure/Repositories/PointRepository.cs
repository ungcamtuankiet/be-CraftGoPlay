using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Repositories
{
    public class PointRepository : GenericRepository<Point>, IPointRepository
    {
        private readonly AppDbContext _dbContext;

        public PointRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Point>> GetPointsAllUserIdAsync()
        {
           return await _dbContext.Point
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Point> GetPointsByUserId(Guid userId)
        {
            return await _dbContext.Point
                .Include(p => p.PointTransactions)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
    }
}
