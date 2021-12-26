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
        public async Task SendProductAddedOrUpdatedEvent(string refererEventId, ProductEntity productEntity, CompetitorProductPrices lastCompetitorPrices)
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
                Strategies = productEntity.Strategies.Select(e => new ProductAddedOrUpdatedEventPayload.ProductStrategy()
                {
                    Id = e.Id,
                    ProductId = e.ProductId,
                    StrategyId = EnumUtils.GetValueFromString<StrategyIds>(e.StrategyId)
                }).ToList(),
                CompetitorConfigs = productEntity.CompetitorConfigs.Select(e => new ProductAddedOrUpdatedEventPayload.ProductCompetitorConfig()
                {
                    Id = e.Id,
                    CompetitorId = EnumUtils.GetValueFromString<CompetitorIds>(e.CompetitorId),
                    Holder = SerializationUtils.Deserialize<ConfigHolder>(e.SerializedHolder)
                }).ToList(),
                LastCompetitorPrices = lastCompetitorPrices,
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
    }
}
