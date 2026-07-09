using E_Commerce.Domain.Contract;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using IDatabase = StackExchange.Redis.IDatabase;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetAsync(string cacheKey, CancellationToken ct)
        {
            var value = await _database.StringGetAsync(cacheKey);
            return value.IsNullOrEmpty ? null : value.ToString();
        }

        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan? ttl = null, CancellationToken ct = default)
        {
            await _database.StringSetAsync(cacheKey, cacheValue, ttl ?? TimeSpan.FromDays(7));
        }
    }
}
