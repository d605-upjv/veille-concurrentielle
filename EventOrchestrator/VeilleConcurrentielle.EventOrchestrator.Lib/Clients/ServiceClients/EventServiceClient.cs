using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.Infrastructure.Core.Configurations;
using VeilleConcurrentielle.Infrastructure.Core.Framework;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.Infrastructure.ServiceClients;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients
{
    public class EventServiceClient : ServiceClientBase, IEventServiceClient
    {
        public EventServiceClient(HttpClient httpClient, IOptions<ServiceUrlsOptions> serviceUrlOptions, ILogger<EventServiceClient> logger)
            : base(httpClient, serviceUrlOptions, logger)
        {
        }

        protected override string Controller => "Events";

        public async Task<PushEventClientResponse<TEvent, TEventPayload>?> PushEventAsync<TEvent, TEventPayload>(PushEventClientRequest<TEvent, TEventPayload> clientRequest) where TEvent : Event<TEventPayload> where TEventPayload : EventPayload
        {
            PushEventServerRequest serverRequest = new PushEventServerRequest();
            serverRequest.EventName = clientRequest.Name;
            serverRequest.Source = clientRequest.Source;
            serverRequest.SerializedPayload = SerializationUtils.Serialize(clientRequest.Payload);
            var serverResponse = await PostAsync<PushEventServerRequest, PushEventServerResponse>(GetServiceUrl(ApplicationNames.EventOrchestrator), serverRequest);
            if (serverResponse != null)
            {
                PushEventClientResponse<TEvent, TEventPayload> clientResponse = new PushEventClientResponse<TEvent, TEventPayload>();
                var eventType = EventResolver.GetEventType<TEventPayload>();
#pragma warning disable CS8601 // Possible null reference assignment.
                clientResponse.Event = Activator.CreateInstance(eventType) as Event<TEventPayload>;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                clientResponse.Event.CreatedAt = serverResponse.Event.CreatedAt;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                clientResponse.Event.Source = serverResponse.Event.Source;
                clientResponse.Event.Id = serverResponse.Event.Id;
                clientResponse.Event.Payload = SerializationUtils.Deserialize<TEventPayload>(serverResponse.Event.SerializedPayload);
                return clientResponse;
            }
            return null;
        }

        public async Task<GetNextEventClientResponse?> GetNextEventAsync()
        {
            var serverResponse = await GetAsync<GetNextEventServerResponse>(GetServiceUrl(ApplicationNames.EventOrchestrator), "next");
            if (serverResponse != null)
            {
                GetNextEventClientResponse clientResponse = new GetNextEventClientResponse()
                {
                    Event = serverResponse.Event
                };
                return clientResponse;
            }
            return null;
        }

        public async Task<ConsumeEventClientResponse?> ConsumeEventAsync(ConsumeEventClientRequest request)
        {
            ConsumeEventServerRequest serverRequest = request;
            var serverResponse = await PostAsync<ConsumeEventServerRequest, ConsumeEventServerResponse>(GetServiceUrl(ApplicationNames.EventOrchestrator), serverRequest, "consume");
            if (serverResponse != null)
            {
                return new ConsumeEventClientResponse()
                {
                    Event = serverResponse.Event
                };
            }
            return null;
        }
    }
}
