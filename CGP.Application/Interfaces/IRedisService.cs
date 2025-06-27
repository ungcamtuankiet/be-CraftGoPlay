using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Interfaces
{
    public interface IRedisService
    {
        Task SetCacheAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null);
        Task<T?> GetCacheAsync<T>(string key);
        Task RemoveCacheAsync(string key);
        Task RemoveByPatternAsync(string pattern);
        Task AddToBlacklistAsync(string token, DateTime expiry);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
