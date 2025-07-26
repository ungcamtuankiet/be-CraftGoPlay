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
    public class ReturnRequestRepository : GenericRepository<ReturnRequest>, IReturnRequestRepository
    {
        private readonly AppDbContext _dbContext;

        public ReturnRequestRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ReturnRequest>> GetAllAsync(int pageIndex, int pageSize, ReturnStatusEnum status)
        {
            return await _dbContext.ReturnRequest
                .Include(rr => rr.Order)
                .Include(rr => rr.User)
                .Where(rr => rr.Status == status && !rr.IsDeleted)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ReturnRequest> GetByOrderIdAsync(Guid orderId)
        {
            return await _dbContext.ReturnRequest
                .Include(rr => rr.Order)
                .Include(rr => rr.User)
                .FirstOrDefaultAsync(rr => rr.OrderId == orderId && !rr.IsDeleted);
        }

        public async Task<List<ReturnRequest>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize, ReturnStatusEnum status)
        {
            return await _dbContext.ReturnRequest
                .Include(rr => rr.Order)
                .Include(rr => rr.User)
                .Where(rr => rr.Status == status && !rr.IsDeleted && rr.UserId == userId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
