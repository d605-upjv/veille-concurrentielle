using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities
{
    [Index(nameof(EventName))]
    public class EventSubscriberEntity : EntityBase
    {
        public string EventName { get; set; }
        public string ApplicationName { get; set; }
    }
}
