using E_Commerce.Application.Contract;
using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Contract.IRepositories;
using E_Commerce.Infrastructure.Data.Contect;
using E_Commerce.Infrastructure.DataSeeding;
using E_Commerce.Infrastructure.Repositories;
using E_Commerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Dependencies
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddInfrastructue(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDBContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUrlService, UrlService>();
            return services;
        }
        
    }

}
