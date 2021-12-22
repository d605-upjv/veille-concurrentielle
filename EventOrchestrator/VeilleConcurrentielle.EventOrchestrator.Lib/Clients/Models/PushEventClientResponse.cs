using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models
{
    public class PushEventClientResponse<TEvent, TEventPayload> where TEvent : Event<TEventPayload> where TEventPayload : EventPayload
    {
        public Event<TEventPayload> Event { get; set; }
    }
}
