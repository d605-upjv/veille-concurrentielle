using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class ConsumeEventServerRequest
    {
        [Required]
        public string EventId { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ApplicationNames ApplicationName { get; set; }
        public string? Reason { get; set; }
    }
}
