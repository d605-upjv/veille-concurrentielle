using VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public class ProductAggregateService : IProductAggregateService
    {
        private readonly IProductAggregateRepository _productAggregateRepository;
        public ProductAggregateService(IProductAggregateRepository productAggregateRepository)
        {
            _productAggregateRepository = productAggregateRepository;
        }

        public async Task StoreProductAsync(string refererEventId, ProductAddedOrUpdatedEventPayload request)
        {
            bool isAdd = false;
            var productEntity = await _productAggregateRepository.GetByIdAsync(request.ProductId);
            if (productEntity == null)
            {
                isAdd = true;
                productEntity = new ProductAggregateEntity();
                productEntity.Id = request.ProductId;
            }
            productEntity.Name = request.ProductName;
            productEntity.Price = request.Price;
            productEntity.Quantity = request.Quantity;
            productEntity.IsActive = request.IsActive;
            productEntity.ImageUrl = request.ImageUrl;
            if (request.LastCompetitorPrices.MinPrice != null)
            {
                productEntity.MinPrice = request.LastCompetitorPrices.MinPrice.Price;
                productEntity.MinPriceQuantity = request.LastCompetitorPrices.MinPrice.Quantity;
                productEntity.MinPriceCompetitorId = request.LastCompetitorPrices.MinPrice.CompetitorId.ToString();
            }
            if (request.LastCompetitorPrices.MaxPrice != null)
            {
                productEntity.MaxPrice = request.LastCompetitorPrices.MaxPrice.Price;
                productEntity.MaxPriceQuantit = request.LastCompetitorPrices.MaxPrice.Quantity;
                productEntity.MaxPriceCompetitorId = request.LastCompetitorPrices.MaxPrice.CompetitorId.ToString();
            }
            productEntity.LastPrices = new List<ProductAggregatePriceEntity>();
            foreach (var competitorPrices in request.LastCompetitorPrices.Prices)
            {
                foreach (var price in competitorPrices.Prices)
                {
                    productEntity.LastPrices.Add(new ProductAggregatePriceEntity()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = productEntity.Id,
                        CompetitorId = competitorPrices.CompetitorId.ToString(),
                        Price = price.Price,
                        Quantity = price.Quantity,
                        CreatedAt = price.CreatedAt
                    });
                }
            }
            productEntity.Strategies = new List<ProductAggregateStrategyEntity>();
            foreach (var strategy in request.Strategies)
            {
                productEntity.Strategies.Add(new ProductAggregateStrategyEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    StrategyId = strategy.StrategyId.ToString(),
                    ProductId = productEntity.Id,
                    Product = productEntity
                });
            }
            productEntity.CompetitorConfigs = new List<ProductAggregateCompetitorConfigEntity>();
            foreach (var competitorConfig in request.CompetitorConfigs)
            {
                var serializedHolder = SerializationUtils.Serialize(competitorConfig.Holder);
                productEntity.CompetitorConfigs.Add(new ProductAggregateCompetitorConfigEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    CompetitorId = competitorConfig.CompetitorId.ToString(),
                    ProductId = productEntity.Id,
                    SerializedHolder = serializedHolder,
                    Product = productEntity
                });
            }
            productEntity.UpdatedAt = request.UpdatedAt;
            productEntity.CreatedAt = request.CreatedAt;
            if (isAdd)
            {
                await _productAggregateRepository.InsertAsync(productEntity);
            }
            else
            {
                await _productAggregateRepository.UpdateAsync(productEntity);
            }
        }
    }
}
