using E_Commerce.Domain.Contract.IRepositories;
using E_Commerce.Domain.Entities.Baskets;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<CustomerBasket?> CreateOrUpdateAsync(CustomerBasket basket, TimeSpan? ttl = null, CancellationToken ct = default)
        {
            var value = JsonSerializer.Serialize(basket);
            var result = await _database.StringSetAsync(basket.Id, value, ttl ?? TimeSpan.FromDays(7));
            return  result ? basket : null;
        }

        public async Task<bool> DeleteAsync(string basketId, CancellationToken ct)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetAsync(string basketId, CancellationToken ct)
        {
            var basket = await _database.StringGetAsync(basketId);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket.ToString());
        }
    }
}
