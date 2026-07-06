using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contract
{
    public interface IBasketService
    {
        Task<Result<BasketDto>> GetAsync(string BasketId, CancellationToken ct = default);
        Task<Result<BasketDto>> CreateOrUpdateAsync(BasketDto basket, TimeSpan? ttl= default, CancellationToken ct = default);
        Task<Result<bool>> DeleteAsync(string BasketId, CancellationToken ct = default);
    }
}
