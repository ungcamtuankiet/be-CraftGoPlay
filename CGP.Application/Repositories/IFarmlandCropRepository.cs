using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IFarmlandCropRepository : IGenericRepository<FarmlandCrop>
    {
        Task<FarmlandCrop?> GetActiveCropAsync(Guid plotId);
        Task<FarmlandCrop?> GetByFarmlandIdAsync(Guid farmlandId);
        Task<FarmlandCrop> GetFarmLandCropWithUserIdAndTileIdAsync(Guid userId, int titleId);
    }
}
