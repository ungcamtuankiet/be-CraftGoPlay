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
        public CraftVillageRepository(AppDbContext dataContext, ICurrentTime currentTime, IClaimsService claimsService) : base(dataContext, currentTime, claimsService)
        {
            _context = dataContext;
        }

        public Task CreateNewCraftVillage(CraftVillage craftVillage)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<CraftVillage>> GetAllCraftVillagesAsync()
        {
            return await _context.CraftVillage.ToListAsync();
        }

        public Task<CraftVillage> GetCraftVillageByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCraftVillage(CraftVillage craftVillage)
        {
            throw new NotImplementedException();
        }
    }
}
