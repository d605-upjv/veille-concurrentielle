using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Core.Services;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public class AggregatorEventProcessor : IEventProcessor
    {
        private readonly IProductAggregateService _productAggregateService;
        private readonly IRecommendationAlertService _recommendationAlertService;
        public AggregatorEventProcessor(IProductAggregateService productAggregateService, IRecommendationAlertService recommendationAlertService)
        {
            _productAggregateService = productAggregateService;
            _recommendationAlertService = recommendationAlertService;
        }
        public async Task ProcessEventAsync(string eventId, EventNames eventName, EventPayload eventPayload)
        {
            switch (eventName)
            {
                case EventNames.ProductAddedOrUpdated:
                    await _productAggregateService.StoreProductAsync(eventId, (ProductAddedOrUpdatedEventPayload)eventPayload);
                    break;
                case EventNames.NewRecommendationPushed:
                    await _recommendationAlertService.StoreNewRecommendationAlertAsync(eventId, (NewRecommendationPushedEventPayload)eventPayload);
                    break;
            }
        }
    }
}
