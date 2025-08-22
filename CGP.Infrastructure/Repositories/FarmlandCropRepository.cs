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
    public class FarmlandCropRepository : GenericRepository<FarmlandCrop>, IFarmlandCropRepository
    {
        private readonly AppDbContext _context;
        public FarmlandCropRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<FarmlandCrop?> GetActiveCropAsync(Guid plotId)
        {
            return await _context.FarmlandCrop
                .Include(fc => fc.Crop)
                .FirstOrDefaultAsync(fc => fc.FarmlandId == plotId && !fc.IsHarvested);
        }
    }
}
