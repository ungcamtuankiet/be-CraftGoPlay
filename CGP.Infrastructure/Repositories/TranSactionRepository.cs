using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Infrastructure.Data;
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
        public Task<List<Transaction>> GetByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Transaction>> GetTransactions()
        {
            throw new NotImplementedException();
        }
    }
}
