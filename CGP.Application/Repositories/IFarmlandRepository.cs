using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IFarmlandRepository : IGenericRepository<FarmLand>
    {
        Task<List<FarmLand>> GetByUserIdAsync(Guid userId);
        Task<FarmLand> GetFarmlandByIdAsync(Guid id);
    }
}
