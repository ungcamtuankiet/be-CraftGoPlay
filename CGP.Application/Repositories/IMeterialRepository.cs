using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IMeterialRepository : IGenericRepository<Meterial>
    {
        public Task<List<Meterial>> GetMeterialsAsync();
        public Task<Meterial> GetMeterialByIdAsync(Guid id);
        public Task<Meterial> GetMeterialByNameAsync(string materialName);
        public Task<List<Meterial>> GetByIdsAsync(List<Guid> ids);
        public Task CreateMeterialAsync(Meterial meterial);
        public Task UpdateMeterialAsync(Meterial meterial);
        public Task DeleteMeterialAsync(Meterial meterial);
    }
}
