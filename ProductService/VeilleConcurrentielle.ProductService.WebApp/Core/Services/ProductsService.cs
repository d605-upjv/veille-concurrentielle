using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.ProductService.Lib.Servers.Models;
using VeilleConcurrentielle.ProductService.WebApp.Core.Models;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventSenderService _eventSenderService;
        private readonly IProductPriceService _productPriceService;
        private readonly IRecommendationService _recommendationService;
        public ProductsService(IProductRepository productRepository, IEventSenderService eventSenderService, IProductPriceService productPriceService, IRecommendationService recommendationService)
        {
            _productRepository = productRepository;
            _eventSenderService = eventSenderService;
            _productPriceService = productPriceService;
            _recommendationService = recommendationService;
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
            productEntity.ShopProductId = request.ShopProductId;
            productEntity.ShopProductUrl = request.ShopProductUrl;
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
            var recommendationResponse = await _recommendationService.GetRecommendationsAsync(new GetRecommendationRequest()
            {
                ProductId = productEntity.Id,
                Price = productEntity.Price,
                Quantity = productEntity.Quantity,
                Strategies = request.Strategies.Select(e => e.Id).ToList(),
                LastCompetitorPrices = lastCompetitorPrices
            });
            await _eventSenderService.SendProductAddedOrUpdatedEvent(refererEventId, productEntity, lastCompetitorPrices, recommendationResponse.Recommendations);
            await _eventSenderService.SendNewRecommendationPushedEvent(refererEventId, productEntity.Id, recommendationResponse.NewRecommendations);
        }

        public async Task<List<ProductToScrap>> GetProductsToScrap()
        {
            List<ProductToScrap> productsToScrap = new List<ProductToScrap>();
            var products = await _productRepository.GetProductsToScrap();
            foreach (var product in products)
            {
                foreach (var competitorConfig in product.CompetitorConfigs)
                {
                    var config = SerializationUtils.Deserialize<ConfigHolder>(competitorConfig.SerializedHolder);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var urlConfig = config.Items.FirstOrDefault(e => e.Key == ConfigHolderKeys.ProductPageUrl.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (urlConfig != null && !string.IsNullOrWhiteSpace(urlConfig.Value))
                    {
                        productsToScrap.Add(new ProductToScrap()
                        {
                            ProductId = product.Id,
                            CompetitorId = EnumUtils.GetValueFromString<CompetitorIds>(competitorConfig.CompetitorId),
                            ProductProfileUrl = urlConfig.Value
                        });
                    }
                }
            }
            return productsToScrap;
        }
    }
}
