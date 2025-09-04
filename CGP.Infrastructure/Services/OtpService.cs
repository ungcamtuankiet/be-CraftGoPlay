using CGP.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly ConcurrentDictionary<Guid, OtpEntry> _otpStore = new ConcurrentDictionary<Guid, OtpEntry>();

        public async Task StoreOtpAsync(Guid userId, string otp, TimeSpan expiration)
        {
            var otpEntry = new OtpEntry
            {
                Otp = otp,
                ExpiryTime = DateTime.UtcNow.AddHours(7).Add(expiration)
            };
            _otpStore[userId] = otpEntry;

            // Simulate async operation
            await Task.CompletedTask;
        }

        public async Task<bool> ValidateOtpAsync(Guid userId, string otp)
        {
            if (_otpStore.TryGetValue(userId, out var otpEntry))
            {
                if (otpEntry.Otp == otp && otpEntry.ExpiryTime > DateTime.UtcNow.AddHours(7))
                {
                    // OTP is valid, remove it from the store
                    _otpStore.TryRemove(userId, out _);
                    return await Task.FromResult(true);
                }
            }

            return await Task.FromResult(false);
        }
    }
}

public class OtpEntry
{
    public string Otp { get; set; }
    public DateTime ExpiryTime { get; set; }
}
