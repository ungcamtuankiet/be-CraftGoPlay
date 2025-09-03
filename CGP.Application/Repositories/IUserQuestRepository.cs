using CGP.Domain.Entities;
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
        Task<UserQuest> GetUserQuest(Guid userId, Guid questId);
        Task<List<UserQuest>> GetUserQuests(Guid userId);
    }
}
