using VeilleConcurrentielle.Infrastructure.Core.Models.Events;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public interface IProductAggregateService
    {
        Task StoreProductAsync(string refererEventId, ProductAddedOrUpdatedEventPayload request);
    }
}