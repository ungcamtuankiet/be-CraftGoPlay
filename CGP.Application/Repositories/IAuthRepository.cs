using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Repositories
{
    public interface IAuthRepository : IGenericRepository<ApplicationUser>
    {
        Task<bool> UpdateRefreshToken(Guid userId, string refreshToken);

        Task<ApplicationUser> GetRefreshToken(string refreshToken);
        Task<bool> DeleteRefreshToken(Guid userId);
    }
}
