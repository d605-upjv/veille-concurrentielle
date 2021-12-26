using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Services;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Core.Services
{
    public class EventOrchestratorEventProcessor : IEventProcessor
    {
        public async Task ProcessEventAsync(string eventId, EventNames eventName, EventPayload eventPayload)
        {
        }
    }
}
