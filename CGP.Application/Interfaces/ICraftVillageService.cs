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
        public Task<CraftVillage> GetCraftVillageByIdAsync(Guid id);
        public Task CreateNewCraftVillage(CraftVillage craftVillage);
        public Task UpdateCraftVillage(CraftVillage craftVillage);
    }
}
