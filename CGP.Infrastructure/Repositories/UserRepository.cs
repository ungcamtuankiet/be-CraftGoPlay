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

        public async Task<IList<ApplicationUser>> GetAllAccount(int pageIndex, int pageSize, StatusEnum? status, RoleEnum? role)
        {
            var query =  _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.CraftVillage)
                .Include(u => u.Wallet)
                .Include(u => u.UserAddresses)
                .AsQueryable();

                if (status.HasValue)
                {
                    switch (status?.ToString().ToLower())
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
                            query = query.Where(p => p.Status == status);
                            break;
                    }
                }

                if (role.HasValue)
                {
                    switch (role?.ToString().ToLower())
                    {
                        case "admin":
                            query = query.Where(x => x.Role.RoleName == RoleEnum.Admin.ToString());
                            break;
                        case "staff":
                            query = query.Where(x => x.Role.RoleName == RoleEnum.Staff.ToString());
                            break;
                        case "artisan":
                            query = query.Where(x => x.Role.RoleName == RoleEnum.Artisan.ToString());
                            break;
                        case "user":
                            query = query.Where(x => x.Role.RoleName == RoleEnum.User.ToString());
                            break;
                        default:
                            query = query.Where(x => x.Role.RoleName == role.ToString());
                            break;
                    }
                }
            return await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(t => t.CreationDate)
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

        public async Task<Dictionary<string, int>> CountUsersByRoleAsync()
        {
            var roleCounts = await _dbContext.User
                .GroupBy(u => u.Role.RoleName)
                .Where(g => new[] { RoleEnum.User.ToString(), RoleEnum.Artisan.ToString(), RoleEnum.Staff.ToString() }.Contains(g.Key))
                .Select(g => new { RoleName = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.RoleName, v => v.Count);

            // Đảm bảo tất cả các role được yêu cầu đều có trong kết quả, kể cả khi số lượng bằng 0
            var result = new Dictionary<string, int>
            {
                { RoleEnum.User.ToString(), 0 },
                { RoleEnum.Artisan.ToString(), 0 },
                { RoleEnum.Staff.ToString(), 0 }
             };

            foreach (var roleCount in roleCounts)
            {
                if (result.ContainsKey(roleCount.Key))
                {
                    result[roleCount.Key] = roleCount.Value;
                }
            }

            return result;
        }

        public async Task<bool> CheckExistUserVoucher(Guid userId, Guid voucherId)
        {
            var userHasVoucher = await _dbContext.User
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Vouchers)
                .AnyAsync(v => v.Id == voucherId);

            if(userHasVoucher)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<ApplicationUser>> GetAllsVoucherByUserId(Guid userId, VoucherTypeEnum voucherTypeEnum)
        {
            return await _dbContext.User
                .Include(u => u.Vouchers.Where(v => v.Type == voucherTypeEnum))
                .Where(u => u.Id == userId)
                .ToListAsync();
        }
    }
}
