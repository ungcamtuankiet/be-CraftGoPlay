using CGP.Contract.DTO.Quest;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IQuestService
    {
        Task<Result<List<ViewQuestDTO>>> GetAllQuests();
        Task<Result<ViewQuestDTO>> GetQuestById(Guid id);
        Task<Result<object>> CreateQuest(CreateQuestDTO createQuestDTO);
    }
}
