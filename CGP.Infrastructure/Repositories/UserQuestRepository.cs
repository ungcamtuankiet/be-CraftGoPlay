using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Repositories
{
    public class UserQuestRepository : GenericRepository<UserQuest>, IUserQuestRepository
    {
        private readonly AppDbContext _context;
        public UserQuestRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<UserQuest> CheckExistQuest(Guid userId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            return await _context.UserQuest
                .Where(uq => uq.UserId == userId && uq.Quest.QuestType == QuestType.DailyLogin && uq.Quest.IsDaily&& uq.CreationDate >= today)
                .FirstOrDefaultAsync();
        }

        public async Task<UserQuest> GetUserQuestAsync(Guid userId, Guid userQuestId)
        {
            return await _context.UserQuest
                .Include(uq => uq.Quest)
                .FirstOrDefaultAsync(uq => uq.Id == userQuestId && uq.UserId == userId);
        }

        public async Task<UserQuest?> GetByUserAndQuestAsync(Guid userId, Guid questId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            return await _context.UserQuest
                .FirstOrDefaultAsync(uq => uq.UserId == userId
                    && uq.QuestId == questId
                    && uq.CreationDate >= today
                    && uq.CreationDate < today.AddDays(1));
        }

        public async Task<List<UserQuest>> GetUserQuests(Guid userId)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);

            var userQuests = await _context.UserQuest
                .Include(uq => uq.Quest)
                .Where(uq => uq.UserId == userId
                             && uq.Quest.IsDaily
                             && uq.CreationDate >= today
                             && uq.CreationDate < today.AddDays(1))
                .ToListAsync();

            return userQuests;
        }

        public async Task<UserQuest> GetByUserAndQuestTypeAsync(Guid userId, QuestType questType)
        {
            var today = DateTime.UtcNow.Date.AddHours(7);
            return await _context.UserQuest
                .FirstOrDefaultAsync(uq => uq.UserId == userId
                    && uq.Quest.QuestType == questType
                    && uq.CreationDate >= today
                    && uq.CreationDate < today.AddDays(1));
        }
    }
}
