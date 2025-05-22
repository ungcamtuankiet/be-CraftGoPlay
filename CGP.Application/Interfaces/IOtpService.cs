using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IOtpService
    {
        Task StoreOtpAsync(Guid userId, string otp, TimeSpan expiration);
        Task<bool> ValidateOtpAsync(Guid userId, string otp);
    }
}
