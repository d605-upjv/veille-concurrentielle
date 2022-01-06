using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class DispatchEventServerRequest
    {
        [Required]
        public string EventId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public ApplicationNames ApplicationName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public EventNames EventName { get; set; }
        [Required]
        public string SerializedPayload { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime DispatchedAt { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventSources Source { get; set; }
    }
}
