using Microsoft.Extensions.DependencyInjection;
using VeilleConcurrentielle.ProductService.Lib.Clients.ServiceClients;

namespace VeilleConcurrentielle.ProductService.Lib.Registries
{
    public static class AppRegistry
    {
        public static void RegisterProductServiceClientDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductServiceClient, ProductServiceClient>();
        }
    }
}
