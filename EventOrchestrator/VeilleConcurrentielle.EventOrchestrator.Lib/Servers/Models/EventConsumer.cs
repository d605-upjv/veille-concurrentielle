using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class EventConsumer
    {
        public string Id { get; set; }
        public ApplicationNames ApplicationName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
