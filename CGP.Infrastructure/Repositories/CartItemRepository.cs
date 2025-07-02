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
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        private readonly AppDbContext _dbContext;

        public CartItemRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId)
        {
            return await _dbContext.CartItem
                .Include(i => i.Cart) 
                .Include(i => i.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(ct => ct.Id == cartItemId);
        }
        public async Task AddCartItemAsync(CartItem item)
        {
            await _dbContext.CartItem.AddAsync(item);
        }
        public async Task UpdateCartItemAsync(CartItem item)
        {
            _dbContext.CartItem.Update(item);
        }
        public async Task RemoveCartItemAsync(CartItem item)
        {
            _dbContext.CartItem.Remove(item);
        }
    }
}
