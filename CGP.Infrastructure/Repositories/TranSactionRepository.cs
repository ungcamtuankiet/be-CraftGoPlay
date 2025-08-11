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
    public class TranSactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly AppDbContext _dbContext;

        public TranSactionRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(Guid userId)
        {
            return await _dbContext.Transaction
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
        }

        public async Task<Transaction> GetTransactionByOrderId(Guid orderId)
        {
            return await _dbContext.Transaction
                .FirstOrDefaultAsync(t => t.OrderId == orderId);
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            return await _dbContext.Transaction
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
        }
    }
}
