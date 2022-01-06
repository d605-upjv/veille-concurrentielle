using VeilleConcurrentielle.Aggregator.WebApp.Core.Models;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public class ProductAggregateService : IProductAggregateService
    {
        private readonly IProductAggregateRepository _productAggregateRepository;
        private readonly ICompetitorRepository _competitorRepository;
        private readonly IStrategyRepository _strategyRepository;
        private readonly IMainShopWebService _mainShopWebService;
        public ProductAggregateService(IProductAggregateRepository productAggregateRepository,
            ICompetitorRepository competitorRepository,
            IStrategyRepository strategyRepository,
            IMainShopWebService mainShopWebService)
        {
            _productAggregateRepository = productAggregateRepository;
            _competitorRepository = competitorRepository;
            _strategyRepository = strategyRepository;
            _mainShopWebService = mainShopWebService;
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
            productEntity.ShopProductId = request.ShopProductId;
            productEntity.ShopProductUrl = request.ShopProductUrl;
            if (request.LastCompetitorPrices.MinPrice != null)
            {
                productEntity.MinPrice = request.LastCompetitorPrices.MinPrice.Price;
                productEntity.MinPriceQuantity = request.LastCompetitorPrices.MinPrice.Quantity;
                productEntity.MinPriceCompetitorId = request.LastCompetitorPrices.MinPrice.CompetitorId.ToString();
            }
            if (request.LastCompetitorPrices.MaxPrice != null)
            {
                productEntity.MaxPrice = request.LastCompetitorPrices.MaxPrice.Price;
                productEntity.MaxPriceQuantity = request.LastCompetitorPrices.MaxPrice.Quantity;
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
            productEntity.Recommendations = new List<ProductAggregateRecommendationEntity>();
            foreach (var recommendation in request.Recommendations)
            {
                productEntity.Recommendations.Add(new ProductAggregateRecommendationEntity()
                {
                    Id = recommendation.Id,
                    CurrentPrice = recommendation.CurrentPrice,
                    Price = recommendation.Price,
                    ProductId = request.ProductId,
                    StrategyId = recommendation.StrategyId.ToString(),
                    CreatedAt = recommendation.CreatedAt
                });
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

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var competitors = await _competitorRepository.GetAllAsync();
            var strategies = await _strategyRepository.GetAllAsync();
            var items = (await _productAggregateRepository.GetAllAsync())
                                    .Select(e => CreateProductFromEntity(e, competitors, strategies))
                                    .ToList();
            return items;
        }

        public async Task<Product?> GetProductbyIdAsync(string productId)
        {
            var competitors = await _competitorRepository.GetAllAsync();
            var strategies = await _strategyRepository.GetAllAsync();
            var item = await _productAggregateRepository.GetByIdAsync(productId);
            if (item != null)
            {
                return CreateProductFromEntity(item, competitors, strategies);
            }
            return null;
        }

        public async Task<GetProductToAddOrEditModels.GetProductToAddResponse> GetProductToAddAsync()
        {
            GetProductToAddOrEditModels.GetProductToAddResponse productToAdd = new GetProductToAddOrEditModels.GetProductToAddResponse();
            var strategies = await _strategyRepository.GetAllAsync();
            productToAdd.AllStrategies = strategies.Select(e => new ProductStrategy()
            {
                StrategyId = EnumUtils.GetValueFromString<StrategyIds>(e.Id),
                StrategyName = e.Name
            }).ToList();
            var competitors = await _competitorRepository.GetAllAsync();
            productToAdd.CompetitorConfigs = competitors.Select(e => new GetProductToAddOrEditModels.CompetitorConfig()
            {
                CompetitorId = EnumUtils.GetValueFromString<CompetitorIds>(e.Id),
                CompetitorName = e.Name,
                ProductUrl = String.Empty,
                LogoUrl = e.LogoUrl
            }).ToList();
            productToAdd.SelectedStrategies = new List<ProductStrategy>();
            return productToAdd;
        }

        public async Task<GetProductToAddOrEditModels.GetProductToEditResponse?> GetProductToEditAsync(string productId)
        {
            var product = await _productAggregateRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return null;
            }
            GetProductToAddOrEditModels.GetProductToEditResponse productToEdit = new GetProductToAddOrEditModels.GetProductToEditResponse()
            {
                AllStrategies = new List<ProductStrategy>(),
                SelectedStrategies = new List<ProductStrategy>(),
                CompetitorConfigs = new List<GetProductToAddOrEditModels.CompetitorConfig>()
            };
            var strategies = await _strategyRepository.GetAllAsync();
            foreach (var strategy in strategies)
            {
                var productStrategy = new ProductStrategy()
                {
                    StrategyId = EnumUtils.GetValueFromString<StrategyIds>(strategy.Id),
                    StrategyName = strategy.Name
                };
                if (product.Strategies.Exists(e => e.StrategyId == strategy.Id))
                {
                    productToEdit.SelectedStrategies.Add(productStrategy);
                }
                productToEdit.AllStrategies.Add(productStrategy);
            }
            var competitors = await _competitorRepository.GetAllAsync();
            foreach(var competitor in competitors)
            {
                var competitorConfig = new GetProductToAddOrEditModels.CompetitorConfig()
                {
                    CompetitorId = EnumUtils.GetValueFromString<CompetitorIds>(competitor.Id),
                    CompetitorName = competitor.Name,
                    ProductUrl = String.Empty,
                    LogoUrl = competitor.LogoUrl,
                };
                var existingCompetitorConfig = product.CompetitorConfigs.FirstOrDefault(e => e.CompetitorId == competitor.Id);
                if (existingCompetitorConfig != null)
                {
                    var holder = SerializationUtils.Deserialize<ConfigHolder>(existingCompetitorConfig.SerializedHolder);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var productUrlConfig = holder.Items.FirstOrDefault(e => e.Key == ConfigHolderKeys.ProductPageUrl.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (productUrlConfig != null)
                    {
                        competitorConfig.ProductUrl = productUrlConfig.Value;
                    }
                }
                productToEdit.CompetitorConfigs.Add(competitorConfig);
            }
            if (!string.IsNullOrWhiteSpace(product.ShopProductUrl) && !string.IsNullOrWhiteSpace(product.ShopProductId))
            {
                var mainShopProduct = await _mainShopWebService.GetProductAsync(product.ShopProductId, product.ShopProductUrl);
                productToEdit.MainShopProduct = mainShopProduct;
            }
            return productToEdit;
        }

        private Product CreateProductFromEntity(ProductAggregateEntity entity, List<CompetitorEntity> competitors, List<StrategyEntity> strategies)
        {
            var competitorIds = Enum.GetValues<CompetitorIds>();
            var lastPricesOfAllCompetitors = new List<ProductCompetitorPrice>();
            foreach (var competitorId in competitorIds)
            {
                var lastPrice = entity.LastPrices
                                    .Where(e => e.CompetitorId == competitorId.ToString())
                                    .OrderByDescending(e => e.CreatedAt)
                                    .FirstOrDefault();
                var productPrice = new ProductCompetitorPrice()
                {
                    CompetitorId = competitorId,
                    CompetitorName = competitors.Where(e => e.Id == competitorId.ToString()).First().Name,
                    CompetitorLogUrl = competitors.Where(e => e.Id == competitorId.ToString()).First().LogoUrl
                };
                if (lastPrice != null)
                {
                    productPrice.CreatedAt = lastPrice.CreatedAt;
                    productPrice.Quantity = lastPrice.Quantity;
                    productPrice.Price = lastPrice.Price;
                }
                lastPricesOfAllCompetitors.Add(productPrice);
            }
            return new Product()
            {
                ProductId = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                IsActive = entity.IsActive,
                ImageUrl = entity.ImageUrl,
                Quantity = entity.Quantity,
                MinPrice = entity.MinPrice,
                MinPriceQuantity = entity.MinPriceQuantity,
                MinPriceCompetitorId = entity.MinPriceCompetitorId,
                MinPriceCompetitorName = entity.MinPriceCompetitorId != null ? competitors.Where(e => e.Id == entity.MinPriceCompetitorId).First().Name : null,
                MinPriceCompetitorLogoUrl = entity.MinPriceCompetitorId != null ? competitors.Where(e => e.Id == entity.MinPriceCompetitorId).First().LogoUrl : null,
                MaxPrice = entity.MaxPrice,
                MaxPriceQuantity = entity.MaxPriceQuantity,
                MaxPriceCompetitorId = entity.MaxPriceCompetitorId,
                MaxPriceCompetitorName = entity.MaxPriceCompetitorId != null ? competitors.Where(e => e.Id == entity.MaxPriceCompetitorId).First().Name : null,
                MaxPriceCompetitorLogoUrl = entity.MaxPriceCompetitorId != null ? competitors.Where(e => e.Id == entity.MaxPriceCompetitorId).First().LogoUrl : null,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                ShopProductId = entity.ShopProductId,
                ShopProductUrl = entity.ShopProductUrl,
                Strategies = entity.Strategies.Select(e => new ProductStrategy()
                {
                    StrategyId = EnumUtils.GetValueFromString<StrategyIds>(e.StrategyId),
                    StrategyName = strategies.First(c => c.Id == e.StrategyId).Name
                }).ToList(),
                CompetitorConfigs = entity.CompetitorConfigs.Select(e => new ProductCompetitorConfig()
                {
                    CompetitorId = EnumUtils.GetValueFromString<CompetitorIds>(e.CompetitorId),
#pragma warning disable CS8601 // Possible null reference assignment.
                    Holder = SerializationUtils.Deserialize<ConfigHolder>(e.SerializedHolder)
#pragma warning restore CS8601 // Possible null reference assignment.
                }).ToList(),
                LastPrices = entity.LastPrices.Select(e => new ProductCompetitorPrice()
                {
                    CompetitorId = EnumUtils.GetValueFromString<CompetitorIds>(e.CompetitorId),
                    Quantity = e.Quantity,
                    Price = e.Price,
                    CreatedAt = e.CreatedAt,
                    CompetitorName = competitors.Where(c => c.Id == e.CompetitorId).First().Name,
                    CompetitorLogUrl = competitors.Where(c => c.Id == e.CompetitorId).First().LogoUrl,
                })
                .OrderByDescending(e => e.CreatedAt)
                .ToList(),
                LastPricesOfAlCompetitors = lastPricesOfAllCompetitors,
                Recommendations = entity.Recommendations.Select(e => new Models.ProductRecommendation()
                {
                    StrategyId = EnumUtils.GetValueFromString<StrategyIds>(e.StrategyId),
                    StrategyName = strategies.First(c => c.Id == e.StrategyId).Name,
                    Price = e.Price,
                    CurrentPrice = e.CurrentPrice,
                    CreatedAt = e.CreatedAt
                }).ToList()
            };
        }
    }
}
