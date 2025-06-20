using CGP.Contract.DTO.CraftVillage;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface ICraftVillageService
    {
        public Task<Result<List<ViewCraftVillageDTO>>> GetAllCraftVillagesAsync();
        public Task<Result<ViewCraftVillageDTO>> GetCraftVillageByIdAsync(Guid id);
        public Task<Result<CraftVillage>> CreateNewCraftVillage(CreateCraftVillageDTO craftVillage);
        public Task<Result<CraftVillage>> UpdateCraftVillage(Guid id, UpdateCraftVillageDTO craftVillage);
        public Task<Result<object>> DeleteCraftVillage(Guid id);
    }
}
