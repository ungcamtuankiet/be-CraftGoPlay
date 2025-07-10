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
    public class CraftSkillRepository : GenericRepository<CraftSkill>, ICraftSkillRepository
    {
        private readonly AppDbContext _dbContext;

        public CraftSkillRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<CraftSkill>> GetAllAsync(string categoryName)
        {
            return await _dbContext.CraftSkill
                .Include(cs => cs.Artisans)
                .ToListAsync();
        }

        public async Task<CraftSkill> GetByNameAsync(string name)
        {
            return await _dbContext.CraftSkill
               .Include(cs => cs.Artisans)
               .FirstOrDefaultAsync(cs => cs.Name.ToLower() == name.ToLower());
        }


        public async Task<CraftSkill> GetCraftSkillByIdAsync(Guid id)
        {
            return await _dbContext.CraftSkill
                .Include(cs => cs.Artisans)
                .FirstOrDefaultAsync(cs => cs.Id == id);
        }
    }
}
