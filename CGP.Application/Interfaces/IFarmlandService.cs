using CGP.Contract.DTO.Farmland;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IFarmlandService
    {
        Task<Result<List<ViewFarmlandDTO>>> GetFarmlandsByUserIdAsync(Guid userId);
        Task<Result<object>> PlantCropAsync(PlantCropDTO plant);
        Task<Result<object>> WaterAsync(WateredCropDTO water);
        Task<Result<object>> PlowAsync(PlowCropDTO plowCropDTO);
        Task<Result<object>> HarvestAsync(HavestCropDTO havest);
    }
}
