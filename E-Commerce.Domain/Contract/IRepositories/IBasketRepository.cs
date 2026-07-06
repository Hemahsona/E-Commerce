using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Contract.IRepositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetAsync(string basketId, CancellationToken ct);
        Task<CustomerBasket?> CreateOrUpdateAsync(CustomerBasket basket, TimeSpan? ttl = default, CancellationToken ct = default);
        Task<bool> DeleteAsync(string basketId, CancellationToken ct);
    }
}
