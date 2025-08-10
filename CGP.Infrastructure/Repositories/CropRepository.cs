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
    public class CropRepository : GenericRepository<Crop>, ICropRepository
    {
        private readonly AppDbContext _context;
        public CropRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<Crop> GetCropsByIdAsync(Guid id)
        {
            return await _context.Crop
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Crop>> GetCropsByUserIdAsync(Guid userId)
        {
            return await _context.Crop
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
    }
}
