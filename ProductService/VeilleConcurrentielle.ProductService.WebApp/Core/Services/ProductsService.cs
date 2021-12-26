using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventSenderService _eventSenderService;
        private readonly IProductPriceService _productPriceService;
        public ProductsService(IProductRepository productRepository, IEventSenderService eventSenderService, IProductPriceService productPriceService)
        {
            _productRepository = productRepository;
            _eventSenderService = eventSenderService;
            _productPriceService = productPriceService;
        }
        public async Task OnAddOrUPdateProductRequestedAsync(string refererEventId, AddOrUPdateProductRequestedEventPayload request)
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
            var lastCompetitorPrices = await _productPriceService.GetLastPricesAsync(productEntity.Id);
            await _eventSenderService.SendProductAddedOrUpdatedEvent(refererEventId, productEntity, lastCompetitorPrices);
        }
    }
}
