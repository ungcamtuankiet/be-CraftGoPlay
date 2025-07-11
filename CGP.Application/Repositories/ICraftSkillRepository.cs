using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface ICraftSkillRepository : IGenericRepository<CraftSkill>
    {
        public Task<CraftSkill> GetByNameAsync(string name);
        public Task<IEnumerable<CraftSkill>> GetAllAsync(string categoryName);
        public Task<CraftSkill> GetCraftSkillByIdAsync(Guid id);
        public Task<List<CraftSkill>> GetByIdsAsyncs(List<Guid> ids);
    }
}
