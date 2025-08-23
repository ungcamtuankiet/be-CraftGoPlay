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

        public async Task<Crop> CheckName(string name)
        {
            return await _context.Crop
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Crop> GetCropsByIdAsync(Guid id)
        {
            return await _context.Crop
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
