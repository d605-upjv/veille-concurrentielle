extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Core.Services;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Core.Services
{
    public class ProductAggregateServiceTests
    {
        private readonly string _refererEventId = "SourceEventId";

        private readonly Mock<ICompetitorRepository> _competitorRepository;
        private readonly Mock<IStrategyRepository> _strategyRepositoryMock;
        private readonly Mock<IMainShopWebService> _mainShopWebServiceMock;

        public ProductAggregateServiceTests()
        {
            _competitorRepository = new Mock<ICompetitorRepository>();
            _competitorRepository.Setup(s => s.GetAllAsync())
                                        .Returns(() =>
                                        {
                                            var competitorIds = Enum.GetValues<CompetitorIds>();
                                            var competitors = competitorIds.Select(e => new CompetitorEntity()
                                            {
                                                Id = e.ToString(),
                                                Name = e.ToString(),
                                                LogoUrl = "https://anyurl"
                                            }).ToList();
                                            return Task.FromResult(competitors);
                                        });
            _strategyRepositoryMock = new Mock<IStrategyRepository>();
            _strategyRepositoryMock.Setup(s => s.GetAllAsync())
                                        .Returns(() =>
                                        {
                                            var strategyIds = Enum.GetValues<StrategyIds>();
                                            var strategies = strategyIds.Select(e => new StrategyEntity()
                                            {
                                                Id = e.ToString(),
                                                Name = e.ToString()
                                            }).ToList();
                                            return Task.FromResult(strategies);
                                        });
            _mainShopWebServiceMock = new Mock<IMainShopWebService>();
        }

        [Fact]
        public async Task StoreProduct_AddProduct_IfDosNotExistYet()
        {
            string productId = "ProductId";
            Mock<IProductAggregateRepository> productRepositoryMock = new Mock<IProductAggregateRepository>();
            IProductAggregateService productsService = new ProductAggregateService(productRepositoryMock.Object, _competitorRepository.Object, _strategyRepositoryMock.Object, _mainShopWebServiceMock.Object);

            productRepositoryMock.Setup(s => s.GetByIdAsync(productId))
                                                .Returns(Task.FromResult<ProductAggregateEntity?>(null));
            productRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<ProductAggregateEntity>()))
                                                .Callback<ProductAggregateEntity>(entity =>
                                                {
                                                    entity.Id = productId;
                                                });

            ProductAddedOrUpdatedEventPayload request = new ProductAddedOrUpdatedEventPayload()
            {
                ProductId = null,
                Strategies = new List<ProductAddedOrUpdatedEventPayload.ProductStrategy>(),
                CompetitorConfigs = new List<ProductAddedOrUpdatedEventPayload.ProductCompetitorConfig>(),
                LastCompetitorPrices = new CompetitorProductPrices()
                {
                    Prices = new List<CompetitorProductPrices.CompetitorItemProductPrices>()
                },
                Recommendations = new List<ProductRecommendation>()
            };
            await productsService.StoreProductAsync(_refererEventId, request);

            productRepositoryMock.Verify(s => s.GetByIdAsync(It.IsAny<string>()), Times.Once());
            productRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<ProductAggregateEntity>()), Times.Once());
            productRepositoryMock.Verify(s => s.UpdateAsync(It.IsAny<ProductAggregateEntity>()), Times.Never());
        }

        [Fact]
        public async Task StoreProduct_AddProduct_KeepProvidedProductId()
        {
            string productId = "ProductId";
            Mock<IProductAggregateRepository> productRepositoryMock = new Mock<IProductAggregateRepository>();
            IProductAggregateService productsService = new ProductAggregateService(productRepositoryMock.Object, _competitorRepository.Object, _strategyRepositoryMock.Object, _mainShopWebServiceMock.Object);

            productRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<ProductAggregateEntity>()))
                                                .Callback<ProductAggregateEntity>(entity =>
                                                {
                                                    entity.Id = productId;
                                                });

            ProductAddedOrUpdatedEventPayload request = new ProductAddedOrUpdatedEventPayload()
            {
                ProductId = productId,
                Strategies = new List<ProductAddedOrUpdatedEventPayload.ProductStrategy>(),
                CompetitorConfigs = new List<ProductAddedOrUpdatedEventPayload.ProductCompetitorConfig>(),
                LastCompetitorPrices = new CompetitorProductPrices()
                {
                    Prices = new List<CompetitorProductPrices.CompetitorItemProductPrices>()
                },
                Recommendations = new List<ProductRecommendation>()
            };
            await productsService.StoreProductAsync(_refererEventId, request);

            Assert.Equal(productId, request.ProductId);
        }

        [Fact]
        public async Task StoreProduct_EditProduct_IfAlreadyExists()
        {
            string productId = "ProductId";
            Mock<IProductAggregateRepository> productRepositoryMock = new Mock<IProductAggregateRepository>();
            IProductAggregateService productsService = new ProductAggregateService(productRepositoryMock.Object, _competitorRepository.Object, _strategyRepositoryMock.Object, _mainShopWebServiceMock.Object);

            productRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                                .Returns(Task.FromResult<ProductAggregateEntity?>(new ProductAggregateEntity()
                                                {
                                                    Id = productId,
                                                }));
            productRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<ProductAggregateEntity>()))
                                                .Callback<ProductAggregateEntity>(entity =>
                                                {
                                                    entity.Id = productId;
                                                });

            ProductAddedOrUpdatedEventPayload request = new ProductAddedOrUpdatedEventPayload()
            {
                ProductId = null,
                Strategies = new List<ProductAddedOrUpdatedEventPayload.ProductStrategy>(),
                CompetitorConfigs = new List<ProductAddedOrUpdatedEventPayload.ProductCompetitorConfig>(),
                LastCompetitorPrices = new CompetitorProductPrices()
                {
                    Prices = new List<CompetitorProductPrices.CompetitorItemProductPrices>()
                },
                Recommendations = new List<ProductRecommendation>()
            };
            await productsService.StoreProductAsync(_refererEventId, request);

            productRepositoryMock.Verify(s => s.GetByIdAsync(It.IsAny<string>()), Times.Once());
            productRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<ProductAggregateEntity>()), Times.Never());
            productRepositoryMock.Verify(s => s.UpdateAsync(It.IsAny<ProductAggregateEntity>()), Times.Once());
        }

        [Fact]
        public async Task GetAllProductsAsync_CallsAppropriateServices()
        {
            Mock<IProductAggregateRepository> productRepositoryMock = new Mock<IProductAggregateRepository>();
            IProductAggregateService productsService = new ProductAggregateService(productRepositoryMock.Object, _competitorRepository.Object, _strategyRepositoryMock.Object, _mainShopWebServiceMock.Object);
            productRepositoryMock.Setup(s => s.GetAllAsync())
                                    .Returns(Task.FromResult(new List<ProductAggregateEntity>()));
            var items = await productsService.GetAllProductsAsync();

            productRepositoryMock.Verify(s => s.GetAllAsync(), Times.Once());
            Assert.NotNull(items);
        }

        [Fact]
        public async Task GetProductbyIdAsync_CallsAppropriateServices()
        {
            string productId = "ProductId";
            Mock<IProductAggregateRepository> productRepositoryMock = new Mock<IProductAggregateRepository>();
            IProductAggregateService productsService = new ProductAggregateService(productRepositoryMock.Object, _competitorRepository.Object, _strategyRepositoryMock.Object, _mainShopWebServiceMock.Object);
            productRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                    .Returns(Task.FromResult<ProductAggregateEntity?>(new ProductAggregateEntity()
                                    {
                                        Strategies = new List<ProductAggregateStrategyEntity>(),
                                        CompetitorConfigs = new List<ProductAggregateCompetitorConfigEntity>(),
                                        LastPrices = new List<ProductAggregatePriceEntity>(),
                                        Recommendations = new List<ProductAggregateRecommendationEntity>()
                                    }));
            var product = await productsService.GetProductbyIdAsync(productId);

            productRepositoryMock.Verify(s => s.GetByIdAsync(productId), Times.Once());
            Assert.NotNull(product);
            Assert.Equal(Enum.GetValues<CompetitorIds>().Length, product.LastPricesOfAlCompetitors.Count);
        }

        [Fact]
        public async Task GetProductToAddAsync_CallsAppropriateServices()
        {
            Mock<IProductAggregateRepository> productRepositoryMock = new Mock<IProductAggregateRepository>();
            IProductAggregateService productsService = new ProductAggregateService(productRepositoryMock.Object, _competitorRepository.Object, _strategyRepositoryMock.Object, _mainShopWebServiceMock.Object);
            var product = await productsService.GetProductToAddAsync();

            _strategyRepositoryMock.Verify(s => s.GetAllAsync(), Times.Once());
            _competitorRepository.Verify(s => s.GetAllAsync(), Times.Once());
            Assert.NotNull(product);
            Assert.NotEmpty(product.AllStrategies);
            Assert.NotEmpty(product.CompetitorConfigs);
            Assert.NotNull(product.SelectedStrategies);
        }

        [Fact]
        public async Task GetProductToEditAsync_CallsAppropriateServices()
        {
            string productId = "ProductId";
            string mainShopProductId = "MainShopProductId";
            string mainShopProductUrl = "https://mainshopurl";
            string shopAProductUrl = "https://anyurl";
            Mock<IProductAggregateRepository> productRepositoryMock = new Mock<IProductAggregateRepository>();
            productRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                                .Returns(Task.FromResult<ProductAggregateEntity?>(new ProductAggregateEntity()
                                                {
                                                    Id = productId,
                                                    Strategies = new List<ProductAggregateStrategyEntity>
                                                    {
                                                        new ProductAggregateStrategyEntity()
                                                        {
                                                            Id = Guid.NewGuid().ToString(),
                                                            ProductId = productId,
                                                            StrategyId = StrategyIds.FivePercentAboveMeanPrice.ToString()
                                                        }
                                                    },
                                                    CompetitorConfigs = new List<ProductAggregateCompetitorConfigEntity>
                                                    {
                                                        new ProductAggregateCompetitorConfigEntity()
                                                        {
                                                            Id = Guid.NewGuid().ToString(),
                                                            ProductId = productId,
                                                            CompetitorId = CompetitorIds.ShopA.ToString(),
                                                            SerializedHolder = SerializationUtils.Serialize(new ConfigHolder()
                                                            {
                                                                Items = new List<ConfigHolder.ConfigItem>
                                                                {
                                                                    new ConfigHolder.ConfigItem()
                                                                    {
                                                                        Key = ConfigHolderKeys.ProductPageUrl.ToString(),
                                                                        Value = shopAProductUrl
                                                                    }
                                                                }
                                                            })
                                                        }
                                                    },
                                                    ShopProductId = mainShopProductId,
                                                    ShopProductUrl = mainShopProductUrl
                                                }));
            _mainShopWebServiceMock.Setup(s => s.GetProductAsync(It.IsAny<string>(), It.IsAny<string>()))
                                                .Returns((string productId, string productUrl) =>
                                                {
                                                    return Task.FromResult<mywebapp.VeilleConcurrentielle.Aggregator.WebApp.Core.Models.MainShopProduct?>(new mywebapp.VeilleConcurrentielle.Aggregator.WebApp.Core.Models.MainShopProduct()
                                                    {
                                                        ShopProductId = mainShopProductId,
                                                        ShopProductUrl = mainShopProductUrl
                                                    });
                                                });
            IProductAggregateService productsService = new ProductAggregateService(productRepositoryMock.Object, _competitorRepository.Object, _strategyRepositoryMock.Object, _mainShopWebServiceMock.Object);
            var product = await productsService.GetProductToEditAsync(productId);

            _strategyRepositoryMock.Verify(s => s.GetAllAsync(), Times.Once());
            _competitorRepository.Verify(s => s.GetAllAsync(), Times.Once());
            _mainShopWebServiceMock.Verify(s => s.GetProductAsync(mainShopProductId, mainShopProductUrl));
            Assert.NotNull(product);
            Assert.NotNull(product.MainShopProduct);
            Assert.NotEmpty(product.AllStrategies);
            Assert.NotEmpty(product.CompetitorConfigs);
            Assert.True(product.SelectedStrategies.Count == 1, "Selected strategy is provided");
            Assert.True(product.CompetitorConfigs.First(e => e.CompetitorId == CompetitorIds.ShopA).ProductUrl == shopAProductUrl, "Competitor config contains configured product url");
        }
    }
}
