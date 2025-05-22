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
    public class AuthRepository : GenericRepository<ApplicationUser>, IAuthRepository
    {
        private readonly AppDbContext _dbContext;

        public AuthRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteRefreshToken(Guid userId)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
            user.RefreshToken = "";
            return await SaveChange();
        }

        public async Task<ApplicationUser> GetRefreshToken(string refreshToken)
        {
            return await _dbContext.User.Include(r => r.Role).FirstOrDefaultAsync(r => r.RefreshToken == refreshToken);
        }
        public async Task<bool> UpdateRefreshToken(Guid userId, string refreshToken)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
            user.RefreshToken = refreshToken;
            return await SaveChange();
        }

        public async Task<bool> SaveChange()
        {
            var save = await _dbContext.SaveChangesAsync();
            return save > 0 && true;
        }
    }
}
