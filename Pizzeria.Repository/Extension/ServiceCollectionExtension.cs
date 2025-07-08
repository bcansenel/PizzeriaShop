using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Infrastructure.Interface;
using Pizzeria.Repository.Interface;
using Pizzeria.Repository.Repository;

namespace Pizzeria.Repository.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
