using E_Commerce.Application.Contract;
using E_Commerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Application.Dependencies
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddAppliction(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}