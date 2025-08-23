using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface ICropRepository : IGenericRepository<Crop>
    {
        public Task<Crop> GetCropsByIdAsync(Guid id);
        public Task<Crop> CheckName(string name);
    }
}
