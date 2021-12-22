using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients
{
    public interface IEventDispatcherServiceClient
    {
        Task<DispatchEventClientResponse> DispatchEvent(DispatchEventClientRequest cllientRequest);
    }
}