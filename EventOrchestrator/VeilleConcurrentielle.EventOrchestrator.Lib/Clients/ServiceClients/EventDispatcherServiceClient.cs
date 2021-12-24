using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.Infrastructure.Core.Configurations;
using VeilleConcurrentielle.Infrastructure.ServiceClients;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients
{
    public class EventDispatcherServiceClient : ServiceClientBase, IEventDispatcherServiceClient
    {
        protected override string Controller => "ReceivedEvents";

        public EventDispatcherServiceClient(HttpClient httpClient, IOptions<ServiceUrlsOptions> serviceUrlOptions, ILogger<EventDispatcherServiceClient> logger) : base(httpClient, serviceUrlOptions, logger)
        {
        }

        public async Task<DispatchEventClientResponse?> DispatchEventAsync(DispatchEventClientRequest clientRequest)
        {
            DispatchEventServerRequest serverRequest = clientRequest;
            var serverResponse = await PostAsync<DispatchEventServerRequest, DispatchEventServerResponse>(GetServiceUrl(clientRequest.ApplicationName), serverRequest); ;
            if (serverResponse != null)
            {
                DispatchEventClientResponse clientResponse = new DispatchEventClientResponse();
                clientResponse.ReceivedEventId = serverResponse.ReceivedEventId;
                return clientResponse;
            }
            return null;
        }
    }
}
