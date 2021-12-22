using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Configurations;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers
{
    public static class WebAppRegistry
    {
        public static void RegisterEventServiceClientDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<ServiceUrlsOptions>(configuration.GetSection(ServiceUrlsOptions.ServiceUrls));
            services.AddHttpClient<IEventDispatcherServiceClient, EventDispatcherServiceClient>();
            services.AddHttpClient<IEventServiceClient, EventServiceClient>();
        }
    }
}
