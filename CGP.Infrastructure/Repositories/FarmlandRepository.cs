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
    public class FarmlandRepository : GenericRepository<FarmLand>, IFarmlandRepository

    {
        private readonly AppDbContext _context;
        public FarmlandRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<List<FarmLand>> GetByUserIdAsync(Guid userId)
        {
            return await _context.FarmLand
                .Include(f => f.User)
                .Include(f => f.FarmlandCrops)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<FarmLand> GetFarmlandByIdAsync(Guid id)
        {
            return await _context.FarmLand
                .Include(f => f.User)
                .Include(f => f.FarmlandCrops)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<FarmLand> GetFarmLandWithUserIdAndTileIdAsync(Guid userId, int titleId)
        {
            return await _context.FarmLand
                .Include(f => f.User)
                .Include(f => f.FarmlandCrops)
                .FirstOrDefaultAsync(f => f.UserId == userId && f.TileId == titleId);
        }
    }
}
