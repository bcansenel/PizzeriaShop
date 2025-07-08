using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Services.Interface;
using Pizzeria.Services.Service;

namespace Pizzeria.Services
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPizzeriaServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<OrderService>();
            services.AddScoped<IProductService, ProductService>();
            IServiceCollection serviceCollection = services.AddScoped<ProductService>();
            return services;
        }
    }
}
