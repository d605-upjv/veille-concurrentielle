using System.Text.Json.Serialization;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models
{
    public class PushEventClientRequest<TEvent, TEventPayload> where TEvent : Event<TEventPayload> where TEventPayload : EventPayload
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventNames Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventSources Source { get; set; }
        public TEventPayload Payload { get; set; }
    }
}
