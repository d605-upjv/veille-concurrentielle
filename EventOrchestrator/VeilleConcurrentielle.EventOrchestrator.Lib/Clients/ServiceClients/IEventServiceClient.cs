﻿using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients
{
    public interface IEventServiceClient
    {
        Task<PushEventClientResponse<TEvent, TEventPayload>> PushEvent<TEvent, TEventPayload>(PushEventClientRequest<TEvent, TEventPayload> clientRequest)
            where TEvent : Event<TEventPayload>
            where TEventPayload : EventPayload;
    }
}