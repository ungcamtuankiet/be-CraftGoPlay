using CGP.Contract.DTO.Auth;
using CGP.Contracts.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IGoogleService
    {
        Task<string> GetUrlLoginWithGoogle();
        Task<string> GoogleCallback(string code);
        Task<Result<object>> RegisterWithGoogle(GoogleUserRequest request);
        Task<Result<object>> GetOrCreateExternalLoginUser(string provider, string key, string email);
        Task<LoginGoogleDTO> AuthenticateGoogleUserAsync(GoogleUserRequest request);
    }
}
