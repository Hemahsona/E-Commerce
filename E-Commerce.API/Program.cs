
using E_Commerce.API.Extentions;
using E_Commerce.Application.Dependencies;
using E_Commerce.Domain.Contract;
using E_Commerce.Infrastructure.Dependencies;
using Microsoft.Extensions.FileProviders;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddInfrastructue(builder.Configuration);
            builder.Services.AddAppliction();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            //builder.Services.AddSwaggerGen();


            var app = builder.Build();
            app.SeedAndMigrationData();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(builder.Environment.ContentRootPath, "Files")),
                RequestPath = "/Files"
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
