using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class PushEventServerRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public EventNames EventName { get; set; }
        [Required]
        public string SerializedPayload { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public EventSources Source { get; set; }
    }
}
