using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contract
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string cacheKey, CancellationToken ct = default);
        Task SetAsync(string cacheKey, object cacheValue, TimeSpan? ttl = null, CancellationToken ct = default);
    }
}
