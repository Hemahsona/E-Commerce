using E_Commerce.Infrastructure.Data.Contect;
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
            return services;
        }
    }

}
