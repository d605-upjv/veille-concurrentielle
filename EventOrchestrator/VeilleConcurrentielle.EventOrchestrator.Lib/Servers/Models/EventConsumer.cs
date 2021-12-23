using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class EventConsumer
    {
        public string Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ApplicationNames ApplicationName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
