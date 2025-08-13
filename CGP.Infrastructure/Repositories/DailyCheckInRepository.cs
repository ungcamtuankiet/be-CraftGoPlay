using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Repositories
{
    public class DailyCheckInRepository : GenericRepository<DailyCheckIn>, IDailyCheckInRepository
    {
        private readonly AppDbContext _context;
        public DailyCheckInRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<DailyCheckIn> IsCheckIn(Guid userId)
        {
            return await _context.DailyCheckIn
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
