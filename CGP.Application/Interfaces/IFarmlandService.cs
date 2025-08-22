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
        Task<Result<bool>> DigAsync(Guid plotId);
        Task<Result<bool>> PlantAsync(Guid plotId, Guid cropId);
        Task<Result<bool>> WaterAsync(Guid plotId);
        Task<Result<bool>> HarvestAsync(Guid plotId);
        Task<Result<bool>> ResetAsync(Guid plotId);
    }
}
