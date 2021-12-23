using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Core.Services
{
    public interface IEventService
    {
        Task<PushEventServerResponse> PushEventAsync(PushEventServerRequest request);
        Task<GetNextEventServerResponse> GetNextEventAsync();
        Task<ConsumeEventServerResponse> ConsumeEventAsync(ConsumeEventServerRequest request);
        Task<Event> GetEventAsync(string eventId);
    }
}