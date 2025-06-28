using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId);
        Task AddCartItemAsync(CartItem item);
        Task UpdateCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(CartItem item);
    }
}
