﻿using Microsoft.Extensions.Options;
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
        protected override string Controller => "Events";

        public EventServiceClient(HttpClient httpClient, IOptions<ServiceUrlsOptions> serviceUrlOptions) : base(httpClient, serviceUrlOptions)
        {
        }

        public async Task<PushEventClientResponse<TEvent, TEventPayload>> PushEvent<TEvent, TEventPayload>(PushEventClientRequest<TEvent, TEventPayload> clientRequest) where TEvent : Event<TEventPayload> where TEventPayload : EventPayload
        {
            PushEventServerRequest serverRequest = new PushEventServerRequest();
            serverRequest.EventName = clientRequest.Name;
            serverRequest.Source = clientRequest.Source;
            serverRequest.SerializedPayload = SerializationUtils.Serialize(clientRequest.Payload);
            var serverResponse = await PostAsync<PushEventServerRequest, PushEventServerResponse>(GetServiceUrl(ApplicationNames.EventOrchestrator), serverRequest);
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
    }
}
