using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class Event
    {
        public string Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventNames Name { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventSources Source { get; set; }
        public string SerializedPayload { get; set; }
        public bool IsConsumed { get; set; }
    }
}
