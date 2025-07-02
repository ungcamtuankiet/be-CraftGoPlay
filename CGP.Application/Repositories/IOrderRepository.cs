using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<Order> GetOrderByIdAsync(Guid id);
        public Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
        public Task<List<Order>> GetListOrderAsync();
    }
}
