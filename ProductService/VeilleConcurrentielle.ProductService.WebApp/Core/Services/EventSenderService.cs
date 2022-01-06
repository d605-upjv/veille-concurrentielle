using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public class EventSenderService : IEventSenderService
    {
        private readonly IEventServiceClient _eventServiceClient;
        public EventSenderService(IEventServiceClient eventServiceClient)
        {
            _eventServiceClient = eventServiceClient;
        }
        public async Task SendProductAddedOrUpdatedEvent(string refererEventId, ProductEntity productEntity, CompetitorProductPrices lastCompetitorPrices, List<ProductRecommendation> recommendations)
        {
            ProductAddedOrUpdatedEventPayload payload = new ProductAddedOrUpdatedEventPayload()
            {
                ProductId = productEntity.Id,
                ProductName = productEntity.Name,
                Price = productEntity.Price,
                Quantity = productEntity.Quantity,
                IsActive = productEntity.IsActive,
                ImageUrl = productEntity.ImageUrl,
                CreatedAt = productEntity.CreatedAt,
                UpdatedAt = productEntity.UpdatedAt,
                ShopProductId = productEntity.ShopProductId,
                ShopProductUrl = productEntity.ShopProductUrl,
                Strategies = productEntity.Strategies.Select(e => new ProductAddedOrUpdatedEventPayload.ProductStrategy()
                {
                    Id = e.Id,
                    StrategyId = EnumUtils.GetValueFromString<StrategyIds>(e.StrategyId)
                }).ToList(),
                CompetitorConfigs = productEntity.CompetitorConfigs.Select(e => new ProductAddedOrUpdatedEventPayload.ProductCompetitorConfig()
                {
                    Id = e.Id,
                    CompetitorId = EnumUtils.GetValueFromString<CompetitorIds>(e.CompetitorId),
                    Holder = SerializationUtils.Deserialize<ConfigHolder>(e.SerializedHolder)
                }).ToList(),
                LastCompetitorPrices = lastCompetitorPrices,
                Recommendations = recommendations,
                RefererEventId = refererEventId
            };
            PushEventClientRequest<ProductAddedOrUpdatedEvent, ProductAddedOrUpdatedEventPayload> request = new PushEventClientRequest<ProductAddedOrUpdatedEvent, ProductAddedOrUpdatedEventPayload>()
            {
                Name = EventNames.ProductAddedOrUpdated,
                Source = EventSources.ProductService,
                Payload = payload
            };
            var response = await _eventServiceClient.PushEventAsync(request);
        }

        public async Task SendNewRecommendationPushedEvent(string refererEventId, string productId, List<ProductRecommendation> newRecommendations)
        {
            foreach(var recommendation in newRecommendations)
            {
                NewRecommendationPushedEventPayload payload = new NewRecommendationPushedEventPayload()
                {
                    ProductId = productId,
                    Recommendation = recommendation,
                    RefererEventId = refererEventId
                };
                PushEventClientRequest<NewRecommendationPushedEvent, NewRecommendationPushedEventPayload> request = new PushEventClientRequest<NewRecommendationPushedEvent, NewRecommendationPushedEventPayload>()
                {
                    Name = EventNames.NewRecommendationPushed,
                    Source = EventSources.ProductService,
                    Payload = payload
                };
                var response = await _eventServiceClient.PushEventAsync(request);
            }
        }
    }
}
