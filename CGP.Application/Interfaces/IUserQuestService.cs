using CGP.Contract.DTO.UserQuest;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IUserQuestService
    {
        Task<Result<List<ViewUserQuestDTO>>> GetUserQuestsAsync(Guid userId);
        Task EnsureDailyQuestAsync(Guid userId);
        Task UpdateProgressAsync(Guid userId, QuestType questType, int amount);
        Task<Result<object>> ClaimDailyRewardAsync(Guid userId, Guid questId);
    }
}
