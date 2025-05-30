using CGP.Contract.DTO.User;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IUserService
    {
        Task<IList<ApplicationUser>> GetALl();
        Task<ApplicationUser> GetByEmail(string email);
        Task UpdateUserAsync(ApplicationUser user);


        Task<UserDTO> GetUserById(Guid id);
        Task<Result<UserDTO>> GetCurrentUserById();
    }
}
