using CGP.Domain.Entities;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<IList<ApplicationUser>> GetALl();
        Task<ApplicationUser?> FindByEmail(string email);

        // Login Google
        Task<ApplicationUser?> FindByLoginAsync(string provider, string key);
        
        Task UpdateAsync(ApplicationUser user);
        Task AddAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<bool> ExistsAsync(Expression<Func<ApplicationUser, bool>> predicate);
        Task<ApplicationUser> GetUserById(Guid userId);
        Task<ApplicationUser> GetAllUserById(Guid id);
        Task<List<ApplicationUser>> GetUsersByRole(int role);
        //Task<User> GetUserByIdWithServiceUsed(Guid userId);

        Task<ApplicationUser> GetUserByVerificationToken(string token);

        //Forget Password
        Task<ApplicationUser> GetUserByResetToken(string resetToken);

    }
}
