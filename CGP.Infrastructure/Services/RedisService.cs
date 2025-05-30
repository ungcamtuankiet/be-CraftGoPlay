using CGP.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CGP.Infrastructure.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _redisDb;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null)
        {
            var jsonString = JsonSerializer.Serialize(value);
            var expiry = absoluteExpireTime ?? TimeSpan.FromMinutes(10);
            await _redisDb.StringSetAsync(key, jsonString, expiry);
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var jsonString = await _redisDb.StringGetAsync(key);
            return jsonString.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(jsonString!);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _redisDb.KeyDeleteAsync(key);
        }

        public async Task AddToBlacklistAsync(string token, DateTime expiry)
        {
            var ttl = expiry - DateTime.UtcNow;
            await _redisDb.StringSetAsync($"blacklist:{token}", "1", ttl);
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _redisDb.KeyExistsAsync($"blacklist:{token}");
        }
    }
}
