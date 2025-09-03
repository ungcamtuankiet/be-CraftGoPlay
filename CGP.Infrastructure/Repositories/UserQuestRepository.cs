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

        public async Task<UserQuest> GetUserQuest(Guid userId, Guid questId)
        {
            return await _context.UserQuest
                .Include(uq => uq.Quest)
                .FirstOrDefaultAsync(uq => uq.Id == questId && uq.UserId == userId);
        }

        public async Task<List<UserQuest>> GetUserQuests(Guid userId)
        {
            var vnTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time");
            var today = vnTime.Date;

            var userQuests = await _context.UserQuest
                .Include(uq => uq.Quest)
                .Where(uq => uq.UserId == userId
                             && uq.Quest.IsDaily
                             && uq.CreationDate >= today
                             && uq.CreationDate < today.AddDays(1))
                .ToListAsync();

            return userQuests;
        }
    }
}
