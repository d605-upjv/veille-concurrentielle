using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Core.Services;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public class AggregatorEventProcessor : IEventProcessor
    {
        private readonly IProductAggregateService _productAggregateService;
        public AggregatorEventProcessor(IProductAggregateService productAggregateService)
        {
            _productAggregateService = productAggregateService;
        }
        public async Task ProcessEventAsync(string eventId, EventNames eventName, EventPayload eventPayload)
        {
            switch (eventName)
            {
                case EventNames.ProductAddedOrUpdated:
                    await _productAggregateService.StoreProductAsync(eventId, (ProductAddedOrUpdatedEventPayload)eventPayload);
                    break;
            }
        }
    }
}
