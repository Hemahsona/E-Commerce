using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Contract
{
    public interface ICacheRepository
    {
        Task<string?> GetAsync(string cacheKey, CancellationToken ct);
        Task SetAsync(string cacheKey, string cacheValue, TimeSpan? ttl = default, CancellationToken ct = default);
    }
}
