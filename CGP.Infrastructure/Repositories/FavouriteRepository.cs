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
    public class FavouriteRepository : GenericRepository<Favourite>, IFavouriteRepository
    {
        private readonly AppDbContext _context;

        public FavouriteRepository(AppDbContext dataContext, ICurrentTime currentTime, IClaimsService claimsService)
            : base(dataContext, currentTime, claimsService)
        {
            _context = dataContext;
        }

        public async Task AddFavourite(Favourite favourite)
        {
            await _context.Favourite.AddAsync(favourite);
        }

        public async Task<Favourite> GetFavouriteByUserAndProduct(Guid userId, Guid productId)
        {
            return await _context.Favourite
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
        }

        public async Task DeleteFavouriteByUserAndProduct(Guid userId, Guid productId)
        {
            var favourite = await GetFavouriteByUserAndProduct(userId, productId);
            if (favourite != null)
            {
                _context.Favourite.Remove(favourite);
            }
        }

        public async Task<List<Favourite>> GetFavouritesByUserId(Guid id)
        {
            return await _context.Favourite
                .Include(fa => fa.User)
                .Include(fa => fa.Product)
                .ThenInclude(fa => fa.ProductImages)
                .Where(fa => fa.UserId == id)
                .ToListAsync();
        }

        public async Task<bool> CheckFavourite(Guid userId, Guid productId)
        {
            return await _context.Favourite
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId);
        }
    }
}
