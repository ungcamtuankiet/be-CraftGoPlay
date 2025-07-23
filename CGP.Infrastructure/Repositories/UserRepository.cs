using CGP.Application.Interfaces;
using CGP.Application.Repositories;
using CGP.Domain.Entities;
using CGP.Domain.Enums;
using CGP.Infrastructure.Data;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CGP.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext,
            ICurrentTime timeService,
            IClaimsService claimsService)
            : base(dbContext,
                  timeService,
                  claimsService)
        {
            _dbContext = dbContext;
        }
        public async Task<IList<ApplicationUser>> GetALl()
        {
            return await _dbContext.User.Where(u => u.RoleId != 1).ToListAsync();
        }
        public async Task<ApplicationUser> FindByEmail(string email)
        {
            return await _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.CraftVillage)
                .Include(u => u.Wallet)
                .Include(u => u.UserAddresses)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IList<ApplicationUser>> GetAllAccountByStatusAsync(int pageIndex, int pageSize, StatusEnum status)
        {
            var query =  _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.CraftVillage)
                .Include(u => u.Wallet)
                .Include(u => u.UserAddresses)
                .AsQueryable();

                if (!string.IsNullOrWhiteSpace(status.ToString()))
                {
                    switch (status.ToString().ToLower())
                    {
                        case "active":
                            query = query.Where(x => x.Status == StatusEnum.Active);
                            break;
                        case "pending":
                            query = query.Where(x => x.Status == StatusEnum.Pending);
                            break;
                        case "inactive":
                            query = query.Where(x => x.Status == StatusEnum.Inactive);
                            break;
                        case "rejected":
                            query = query.Where(x => x.Status == StatusEnum.Rejected);
                            break;
                        default:
                            query = query.Where(p => p.Status == StatusEnum.Active);
                            break;
                    }
                }
                else
                {
                    query = query = query.Where(p => p.Status == status);
                }
            return await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(ApplicationUser user)
        {
            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            _dbContext.User.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _dbContext.User.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _dbContext.User.AnyAsync(predicate);
        }

        public async Task<ApplicationUser> GetUserById(int userId)
        {
            var user = await _dbContext.User.FindAsync(userId);
            return user;
        }

        public async Task<ApplicationUser> GetAllUserById(Guid id)
        {
            return await _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.CraftVillage)
                .Include(u => u.Wallet)
                .Include(u => u.UserAddresses)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<ApplicationUser> GetUserById(Guid userId)
        {
            return await _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.CraftVillage)
                .Include(u => u.Wallet)
                .Include(u => u.UserAddresses)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ApplicationUser> GetUserByVerificationToken(string token)
        {
            return await _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.CraftVillage)
                .Include(u => u.Wallet)
                .Include(u => u.UserAddresses)
                .FirstOrDefaultAsync(u => u.VerificationToken == token);
        }
        public async Task<ApplicationUser> GetUserByResetToken(string resetToken)
        {
            return await _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.CraftVillage)
                .Include(u => u.Wallet)
                .Include(u => u.UserAddresses)
                .FirstOrDefaultAsync(u => u.ResetToken == resetToken);
        }

        public async Task<List<ApplicationUser>> GetUsersByRole(int role)
        {
            return await _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.CraftVillage)
                .Include(u => u.Wallet)
                .Include(u => u.UserAddresses)
                .Where(u => u.RoleId == role).ToListAsync();
        }

        public async Task<ApplicationUser?> FindByLoginAsync(string provider, string key)
        {
            return await _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.CraftVillage)
                .Include(u => u.Wallet)
                .Include(u => u.UserAddresses)
            .Where(u => u.Provider == provider && u.ProviderKey == key)
            .FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser?> FindByPhoneNoAsync(string phoneNo)
        {
            return await _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.CraftVillage)
                .Include(u => u.Wallet)
                .Include(u => u.UserAddresses)
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNo);
        }
    }
}
