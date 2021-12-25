using System.Linq;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventServiceClient _eventServiceClient;
        private readonly IProductPriceService _productPriceService;
        public ProductsService(IProductRepository productRepository, IEventServiceClient eventServiceClient, IProductPriceService productPriceService)
        {
            _productRepository = productRepository;
            _eventServiceClient = eventServiceClient;
            _productPriceService = productPriceService;
        }
        public async Task StoreProductAsync(string refererEventId, AddOrUPdateProductRequestedEventPayload request)
        {
            ProductEntity? productEntity = null;
            bool isAdd = false;
            if (!string.IsNullOrWhiteSpace(request.ProductId))
            {
                productEntity = await _productRepository.GetByIdAsync(request.ProductId);
            }
            if (productEntity == null)
            {
                isAdd = true;
                productEntity = new ProductEntity();
                productEntity.Id = request.ProductId ?? Guid.NewGuid().ToString();
            }
            productEntity.Name = request.ProductName;
            productEntity.Price = request.Price;
            productEntity.Quantity = request.Quantity;
            productEntity.IsActive = request.IsActive;
            productEntity.ImageUrl = request.ImageUrl;
            productEntity.Strategies = new List<StrategyEntity>();
            foreach (var strategy in request.Strategies)
            {
                productEntity.Strategies.Add(new StrategyEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    StrategyId = strategy.Id.ToString(),
                    ProductId = productEntity.Id,
                    Product = productEntity
                });
            }
            productEntity.CompetitorConfigs = new List<CompetitorConfigEntity>();
            foreach (var competitorConfig in request.CompetitorConfigs)
            {
                var serializedHolder = SerializationUtils.Serialize(competitorConfig.Holder);
                productEntity.CompetitorConfigs.Add(new CompetitorConfigEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    CompetitorId = competitorConfig.CompetitorId.ToString(),
                    ProductId = productEntity.Id,
                    SerializedHolder = serializedHolder,
                    Product = productEntity
                });
            }
            productEntity.UpdatedAt = DateTime.Now;
            if (isAdd)
            {
                productEntity.CreatedAt = DateTime.Now;
                await _productRepository.InsertAsync(productEntity);
            }
            else
            {
                await _productRepository.UpdateAsync(productEntity);
            }
            var minPrice = await _productPriceService.GetMinPriceAsync(productEntity.Id);
            var maxPrice = await _productPriceService.GetMaxPriceAsync(productEntity.Id);
            await PushProductAddedOrUpdatedEvent(refererEventId, productEntity, minPrice, maxPrice);
        }

        private async Task PushProductAddedOrUpdatedEvent(string refererEventId, ProductEntity productEntity, ProductPrice? minPrice, ProductPrice? maxPrice)
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
                MinPrice = minPrice,
                MaxPrice = maxPrice,
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
