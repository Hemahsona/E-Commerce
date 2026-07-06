using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Contract.IRepositories;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class BasketService(IBasketRepository basketRepository) : IBasketService
    {
        public async Task<Result<BasketDto>> CreateOrUpdateAsync(BasketDto basket, TimeSpan? ttl = null, CancellationToken ct = default)
        {
            var customerBasket = new CustomerBasket
            {
                Id = basket.Id,
                Items = basket.Items.Select(i => new BasketItem
                {
                    Id = i.Id,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    PictureUrl = i.PictureUrl
                }).ToList()
            };
            var result = await basketRepository.CreateOrUpdateAsync(customerBasket, ttl, ct);
            return result == null ? Result<BasketDto>.Fail(Error.Failure("Failed to create basket"))
                : basket;
        }

        public async Task<Result<bool>> DeleteAsync(string BasketId, CancellationToken ct = default)
        {
            var result = await basketRepository.DeleteAsync(BasketId, ct);
            return result ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("Failed to delete basket"));
        }

        public async Task<Result<BasketDto>> GetAsync(string BasketId, CancellationToken ct = default)
        {
            var result = await basketRepository.GetAsync(BasketId, ct);
            var basketDto = result == null ? null : new BasketDto
            {
                Id = result.Id,
                Items = result.Items.Select(i => new BasketItemDto
                {
                    Id = i.Id,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    PictureUrl = i.PictureUrl
                }).ToList()
            };
            return result == null ? Result<BasketDto>.Fail(Error.NotFound("Basket not found")) 
                : basketDto;
        }
    }
}

