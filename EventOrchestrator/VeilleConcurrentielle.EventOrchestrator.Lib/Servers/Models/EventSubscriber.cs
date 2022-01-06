using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class EventSubscriber
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ApplicationNames ApplicationName { get; set; }
    }
}
