using CGP.Contract.DTO.CraftSkill;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface ICraftSkillService
    {
        public Task<Result<IEnumerable<ViewCraftSkillDTO>>> GetAllCraftSkillsAsync();
        public Task<Result<ViewCraftSkillDTO>> GetCraftSkillByIdAsync(Guid id);
        public Task<Result<object>> CreateNewCraftSkill(CreateCraftSkillDTO createCraftSkillDTO);
        public Task<Result<object>> UpdateNewCraftSkill(UpdateCraftSkillDTO updateCraftSkillDTO);
        public Task<Result<object>> DeleteCraftSkill(Guid id);
    }
}
