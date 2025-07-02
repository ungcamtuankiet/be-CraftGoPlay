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
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly AppDbContext _dbContext;

        public CartRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _dbContext.Cart.AddAsync(cart);
        }

        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            return await _dbContext.Cart
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .ThenInclude(i => i.ProductImages)
            .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCheckedOut);
        }
    }
}
