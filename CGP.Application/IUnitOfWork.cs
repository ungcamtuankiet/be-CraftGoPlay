using CGP.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application
{
    public interface IUnitOfWork
    {

        IAuthRepository authRepository { get; }
        IUserRepository userRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}
