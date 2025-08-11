using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        public Task<List<Transaction>> GetTransactionsByUserIdAsync(Guid userId);
        public Task<Transaction> GetTransactionByOrderId(Guid orderId);
        public Task<List<Transaction>> GetTransactions();
    }
}
