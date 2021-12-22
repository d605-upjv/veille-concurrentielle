using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers
{
    public static class WebAppRegistry
    {
        public static void RegisterEventServiceClientDependencies(this IServiceCollection services, string eventServiceUrl)
        {
            services.AddHttpClient<IEventServiceClient, EventServiceClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(eventServiceUrl);
            });
        }
    }
}
