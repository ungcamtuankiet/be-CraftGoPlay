using CGP.Domain.Entities;
using CGP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IUserQuestRepository : IGenericRepository<UserQuest>
    {
        Task<UserQuest> CheckExistQuest(Guid userId);
        Task<List<UserQuest>> GetUserQuests(Guid userId);
        Task<UserQuest> GetUserQuestAsync(Guid userId, Guid userQuestId);
        Task<UserQuest> GetByUserAndQuestTypeAsync(Guid userId, QuestType questType);
    }
}
