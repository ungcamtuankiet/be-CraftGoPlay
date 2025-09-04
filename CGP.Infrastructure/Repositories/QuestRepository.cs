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
    public class QuestRepository : GenericRepository<Quest>, IQuestRepository
    {
        private readonly AppDbContext _context;
        public QuestRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<List<Quest>> GetDailyQuests()
        {
            return await _context.Quest
            .Where(q => q.IsDaily)
            .ToListAsync();
        }
    }
}
