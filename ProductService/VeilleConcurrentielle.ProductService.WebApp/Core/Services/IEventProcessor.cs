using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public interface IEventProcessor
    {
        Task ProcessEventAsync(string eventId, EventNames eventName, EventPayload eventPayload);
    }
}