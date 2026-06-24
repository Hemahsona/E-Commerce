using E_Commerce.Domain.Contract;
using System.Runtime.CompilerServices;

namespace E_Commerce.API.Extentions
{
    public static class WebAppExtentions
    {
        public static async Task<WebApplication> SeedAndMigrationData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            await seeder.SeedDataAsync();
            return app;
        }
    }
}
