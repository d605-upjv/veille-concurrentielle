using System.Text.Json.Serialization;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models
{
    public abstract class Event<TEventPayload> where TEventPayload : EventPayload
    {
        public string Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public abstract EventNames Name { get; }
        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventSources Source { get; set; }
        public TEventPayload Payload { get; set; }
    }
}
