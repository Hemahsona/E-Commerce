
using E_Commerce.API.Extentions;
using E_Commerce.Domain.Contract;
using E_Commerce.Infrastructure.Dependencies;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddInfrastructue(builder.Configuration);
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();
            app.SeedAndMigrationData();
            //using var scope = app.Services.CreateScope();
            //var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            //await seeder.SeedDataAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
