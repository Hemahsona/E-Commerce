using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data.Contect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class CatalogDataSeeder(StoreDBContext dbContext, ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);

                if (PendingMigrations.Any())
                    await dbContext.Database.MigrateAsync(ct);

                var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");

                await SeedIfEmptyAsync<ProductBrand, int>(seedRoot, "brands.json", ct);
                await SeedIfEmptyAsync<ProductType, int>(seedRoot, "types.json", ct);
                await SeedIfEmptyAsync<Product, int>(seedRoot, "products.json", ct);

                int result = await dbContext.SaveChangesAsync(ct);
                if (result > 0)
                    logger.LogInformation($"{result} Rows Added");
                else
                    logger.LogInformation($"DataBAse already saved");

            }
            catch { }
        }
        public async Task SeedIfEmptyAsync<T, TKey>(string rootPath, string fileName, CancellationToken ct) where T : BaseEntity<TKey>
        {
            if (await dbContext.Set<T>().AnyAsync())
            {
                logger.LogInformation("Table already has data");
                return;
            }
            var filePath = Path.Combine(rootPath, fileName);
            if (!File.Exists(filePath))
            {
                logger.LogWarning("file path not found");
                return;
            }
            using var fileStream = File.OpenRead(filePath);
            var option = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            var items = await JsonSerializer.DeserializeAsync<List<T>>(fileStream, option, ct);
            if (items?.Any() ?? false)
                dbContext.Set<T>().AddRange(items);
        }
    }
}