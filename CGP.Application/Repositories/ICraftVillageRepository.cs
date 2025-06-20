using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface ICraftVillageRepository : IGenericRepository<CraftVillage>
    {
        public Task<ICollection<CraftVillage>> GetAllCraftVillagesAsync();
        public Task<CraftVillage> GetCraftVillageByIdAsync(Guid id);
        public Task CreateNewCraftVillage(CraftVillage craftVillage);
        public Task UpdateCraftVillage(CraftVillage craftVillage);
        
    }
}
