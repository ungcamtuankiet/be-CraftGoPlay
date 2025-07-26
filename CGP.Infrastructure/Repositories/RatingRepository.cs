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
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        private readonly AppDbContext _dbContext;

        public RatingRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Rating>> GetRatingsByArtisanIdAsync(Guid artisanId, int pageIndex, int pageSize)
        {
            return await _dbContext.Rating
                .Where(r => r.Product.Artisan_id == artisanId)
                .OrderByDescending(r => r.Star)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsByProductIdAsync(Guid productId, int pageIndex, int pageSize, int star)
        {
            return await _dbContext.Rating
                .Where(r => r.ProductId == productId && r.Star == star)
                .OrderByDescending(r => r.CreationDate)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(Guid userId, int pageIndex, int pageSize)
        {
            return await _dbContext.Rating
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Star)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> HasPurchased(Guid userId, Guid productId)
        {
            bool hasPurchased = await _dbContext.OrderItem
                .Include(oi => oi.Order)
                .AnyAsync(oi =>oi.Order.UserId == userId &&
                    oi.ProductId == productId &&
                    oi.Order.Status == OrderStatusEnum.Completed);

            return hasPurchased;
        }

        public async Task<bool> CheckRated(Guid userId, Guid productId)
        {
            return await _dbContext.Rating
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }
    }
}
