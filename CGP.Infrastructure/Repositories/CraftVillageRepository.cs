using CGP.Application.Common;
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
    public class CraftVillageRepository : GenericRepository<CraftVillage>, ICraftVillageRepository
    {
        private readonly AppDbContext _context;

        public CraftVillageRepository(AppDbContext dataContext, ICurrentTime currentTime, IClaimsService claimsService)
            : base(dataContext, currentTime, claimsService)
        {
            _context = dataContext;
        }

        public Task CreateNewCraftVillage(CraftVillage craftVillage)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<CraftVillage>> GetAllCraftVillagesAsync()
        {
            return await _context.CraftVillage.AsNoTracking().ToListAsync();
        }

        public async Task<CraftVillage> GetByIdAsync(Guid id)
        {
            return await _context.CraftVillage.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<CraftVillage> GetByNameAsync(string name)
        {
            return await _context.CraftVillage.AsNoTracking().FirstOrDefaultAsync(v => v.Village_Name == name);
        }

        public Task<CraftVillage> GetCraftVillageByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCraftVillage(CraftVillage craftVillage)
        {
            var entry = _context.Entry(craftVillage);
            if (entry.State == EntityState.Detached)
            {
                var existingEntity = await _context.CraftVillage.FindAsync(craftVillage.Id);
                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).CurrentValues.SetValues(craftVillage);
                }
                else
                {
                    _context.CraftVillage.Update(craftVillage);
                }
            }
            else
            {
                entry.State = EntityState.Modified;
            }
        }
    }
}