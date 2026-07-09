using E_Commerce.Application.Contract;
using E_Commerce.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace E_Commerce.Application.Services
{
    internal class CacheService(ICacheRepository repository) : ICacheService
    {
        public async Task<string?> GetAsync(string cacheKey, CancellationToken ct = default)
        {
            return await repository.GetAsync(cacheKey, ct);
        }

        public async Task SetAsync(string cacheKey, object cacheValue, TimeSpan? ttl = null, CancellationToken ct = default)
        {
            var JsonVaule = JsonSerializer.Serialize(cacheValue, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await repository.SetAsync(cacheKey, JsonVaule, ttl, ct);
        }
    }
}
