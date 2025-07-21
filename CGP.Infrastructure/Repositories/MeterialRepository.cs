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
    public class MeterialRepository : GenericRepository<Meterial>, IMeterialRepository
    {
        private readonly AppDbContext _context;

        public MeterialRepository(AppDbContext dataContext, ICurrentTime currentTime, IClaimsService claimsService) : base(dataContext, currentTime, claimsService)
        {
            _context = dataContext;
        }
        public async Task<List<Meterial>> GetMeterialsAsync()
        {
            return await _context.Meterial
                .Include(m => m.Products)
                .ToListAsync();
        }

        public async Task<List<Meterial>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Meterial
            .Where(m => ids.Contains(m.Id))
            .ToListAsync();
        }

        public async Task<Meterial> GetMeterialByIdAsync(Guid id)
        {
            return await _context.Meterial
                .Include(m => m.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateMeterialAsync(Meterial meterial)
        {
            await _context.Meterial.AddAsync(meterial);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMeterialAsync(Meterial meterial)
        {
            _context.Meterial.Remove(meterial);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMeterialAsync(Meterial meterial)
        {
            _context.Meterial.Update(meterial);
            await _context.SaveChangesAsync();
        }

        public async Task<Meterial> GetMeterialByNameAsync(string materialName)
        {
            return await _context.Meterial
                .Include(m => m.Products)
                .FirstOrDefaultAsync(m => m.Name.ToLower() == materialName.ToLower());
        }
    }
}
