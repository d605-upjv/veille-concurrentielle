using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Core.Services
{
    public interface IEventService
    {
        Task<PushEventServerResponse> PushEventAsync(PushEventServerRequest request);
    }
}