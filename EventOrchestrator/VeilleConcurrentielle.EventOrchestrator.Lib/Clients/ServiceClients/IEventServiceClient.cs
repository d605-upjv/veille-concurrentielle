using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients
{
    public interface IEventServiceClient
    {
        Task<PushEventClientResponse<TEvent, TEventPayload>?> PushEventAsync<TEvent, TEventPayload>(PushEventClientRequest<TEvent, TEventPayload> clientRequest)
            where TEvent : Event<TEventPayload>
            where TEventPayload : EventPayload;
        Task<GetNextEventClientResponse?> GetNextEventAsync();
        Task<ConsumeEventClientResponse?> ConsumeEventAsync(ConsumeEventClientRequest request);
    }
}