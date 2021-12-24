using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Configurations;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Registries
{
    public static class AppRegistry
    {
        public static void RegisterEventServiceClientDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceUrlsOptions>(configuration.GetSection(ServiceUrlsOptions.ServiceUrls));
            services.AddHttpClient<IEventDispatcherServiceClient, EventDispatcherServiceClient>();
            services.AddHttpClient<IEventServiceClient, EventServiceClient>();
        }
    }
}
