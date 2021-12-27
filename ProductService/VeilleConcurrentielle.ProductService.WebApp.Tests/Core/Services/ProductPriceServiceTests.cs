extern alias mywebapp;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Configurations;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Models;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Core.Services
{
    public class ProductPriceServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICompetitorPriceRepository> _competitorPriceRepositoryMock;
        private readonly Mock<IEventSenderService> _eventSenderServiceMock;
        private readonly Mock<ILogger<ProductPriceService>> _logerMock;
        private readonly IOptions<ProductPriceOptions> _productPriceOptions;
        private readonly Mock<IRecommendationService> _recommendationServiceMock;

        public ProductPriceServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _competitorPriceRepositoryMock = new Mock<ICompetitorPriceRepository>();
            _eventSenderServiceMock = new Mock<IEventSenderService>();
            _logerMock = new Mock<ILogger<ProductPriceService>>();
            _productPriceOptions = Options.Create(new ProductPriceOptions()
            {
                HistoryPriceCount = 10
            });
            _recommendationServiceMock = new Mock<IRecommendationService>();
            _recommendationServiceMock.Setup(s => s.GetRecommendationsAsync(It.IsAny<GetRecommendationRequest>()))
                                        .Returns(Task.FromResult(new GetRecommendationResponse()
                                        {
                                            Recommendations = new List<ProductRecommendation>(),
                                            NewRecommendations = new List<ProductRecommendation>()
                                        }));
        }

        [Fact]
        public async Task OnPriceIdentifedAsync_IfIsDifferentFromLastPrice_PushNewPrice()
        {
            string productId = "ProductId";
            string eventid = "PreviousEventid";

            _productRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<ProductEntity?>(new ProductEntity()
                                        {
                                            Id = productId,
                                            Strategies = new List<StrategyEntity>()
                                        }));
            _competitorPriceRepositoryMock.Setup(s => s.IsDifferentFromLastPriceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                                            .Returns(Task.FromResult(true));
            _competitorPriceRepositoryMock.Setup(s => s.GetLastPricesAsync(productId, It.IsAny<string>(), _productPriceOptions.Value.HistoryPriceCount))
                                            .Returns(Task.FromResult(new List<CompetitorPriceEntity>()));

            IProductPriceService productPriceService = new ProductPriceService(_competitorPriceRepositoryMock.Object, _eventSenderServiceMock.Object, _productRepositoryMock.Object, _logerMock.Object, _productPriceOptions, _recommendationServiceMock.Object);
            await productPriceService.OnPriceIdentifedAsync(eventid, new PriceIdentifiedEventPayload()
            {
                ProductId = productId,
                RefererEventId = eventid
            });

            _productRepositoryMock.Verify(s => s.GetByIdAsync(productId), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.IsDifferentFromLastPriceAsync(productId, It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<CompetitorPriceEntity>()), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.GetLastPricesAsync(productId, It.IsAny<string>(), _productPriceOptions.Value.HistoryPriceCount), Times.Exactly(Enum.GetValues<CompetitorIds>().Length));
            _recommendationServiceMock.Verify(s => s.GetRecommendationsAsync(It.IsAny<GetRecommendationRequest>()), Times.Once());
            _eventSenderServiceMock.Verify(s => s.SendProductAddedOrUpdatedEvent(eventid, It.IsAny<ProductEntity>(), It.IsAny<CompetitorProductPrices>(), It.IsAny<List<ProductRecommendation>>()), Times.Once());
            _eventSenderServiceMock.Verify(s => s.SendNewRecommendationPushedEvent(eventid, productId, It.IsAny<List<ProductRecommendation>>()), Times.Once());
        }

        [Fact]
        public async Task OnPriceIdentifedAsync_Abort_IfProductIdIsUnknown()
        {
            string productId = "ProductId";
            string eventid = "PreviousEventid";

            _productRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<ProductEntity?>(null));

            IProductPriceService productPriceService = new ProductPriceService(_competitorPriceRepositoryMock.Object, _eventSenderServiceMock.Object, _productRepositoryMock.Object, _logerMock.Object, _productPriceOptions, _recommendationServiceMock.Object);
            await productPriceService.OnPriceIdentifedAsync(eventid, new PriceIdentifiedEventPayload()
            {
                ProductId = productId,
                RefererEventId = eventid
            });

            _productRepositoryMock.Verify(s => s.GetByIdAsync(productId), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.IsDifferentFromLastPriceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never());
            _competitorPriceRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<CompetitorPriceEntity>()), Times.Never());
            _eventSenderServiceMock.Verify(s => s.SendProductAddedOrUpdatedEvent(eventid, It.IsAny<ProductEntity>(), It.IsAny<CompetitorProductPrices>(), It.IsAny<List<ProductRecommendation>>()), Times.Never());
            _eventSenderServiceMock.Verify(s => s.SendNewRecommendationPushedEvent(eventid, productId, It.IsAny<List<ProductRecommendation>>()), Times.Never());
        }

        [Fact]
        public async Task OnPriceIdentifedAsync_Abort_IfNotDifferentFromLastPrice()
        {
            string productId = "ProductId";
            string eventid = "PreviousEventid";

            _productRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<ProductEntity?>(new ProductEntity()
                                        {
                                            Id = productId
                                        }));
            _competitorPriceRepositoryMock.Setup(s => s.IsDifferentFromLastPriceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                                            .Returns(Task.FromResult(false));

            IProductPriceService productPriceService = new ProductPriceService(_competitorPriceRepositoryMock.Object, _eventSenderServiceMock.Object, _productRepositoryMock.Object, _logerMock.Object, _productPriceOptions, _recommendationServiceMock.Object);
            await productPriceService.OnPriceIdentifedAsync(eventid, new PriceIdentifiedEventPayload()
            {
                ProductId = productId,
                RefererEventId = eventid
            });

            _productRepositoryMock.Verify(s => s.GetByIdAsync(productId), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.IsDifferentFromLastPriceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<CompetitorPriceEntity>()), Times.Never());
            _eventSenderServiceMock.Verify(s => s.SendProductAddedOrUpdatedEvent(eventid, It.IsAny<ProductEntity>(), It.IsAny<CompetitorProductPrices>(), It.IsAny<List<ProductRecommendation>>()), Times.Never());
            _eventSenderServiceMock.Verify(s => s.SendNewRecommendationPushedEvent(eventid, productId, It.IsAny<List<ProductRecommendation>>()), Times.Never());
        }

        [Fact]
        public async Task GetLastPricesAsync_CheckMinMaxPrices()
        {
            string productId = "ProductId";
            Dictionary<CompetitorIds, List<CompetitorPriceEntity>> priceEntities = new Dictionary<CompetitorIds, List<CompetitorPriceEntity>>();
            var competitorIds = Enum.GetValues<CompetitorIds>();
            Random random = new Random();
            List<CompetitorPriceEntity> latestPricesByCompetitor = new List<CompetitorPriceEntity>();
            foreach (var competitorId in competitorIds)
            {
                List<CompetitorPriceEntity> prices = new List<CompetitorPriceEntity>();
                for (int i = 1; i <= _productPriceOptions.Value.HistoryPriceCount; i++)
                {
                    prices.Add(new CompetitorPriceEntity()
                    {
                        CompetitorId = competitorId.ToString(),
                        ProductId = productId,
                        Price = random.Next(10, 1000),
                        Quantity = 10,
                        CreatedAt = DateTime.Now.AddDays(-i)
                    });
                }
                prices = prices.OrderByDescending(e => e.CreatedAt).ToList();
                latestPricesByCompetitor.Add(prices.First());
                priceEntities.Add(competitorId, prices);
            }
            var minPrice = latestPricesByCompetitor.OrderBy(e => e.Price).First();
            var maxPrice = latestPricesByCompetitor.OrderByDescending(e => e.Price).First();

            _competitorPriceRepositoryMock.Reset();
            _competitorPriceRepositoryMock.Setup(s => s.GetLastPricesAsync(productId, It.IsAny<string>(), _productPriceOptions.Value.HistoryPriceCount))
                                            .Returns((string productId_, string competitorId_, int priceCount_) =>
                                                        Task.FromResult(priceEntities[EnumUtils.GetValueFromString<CompetitorIds>(competitorId_)]
                                                    ));
            IProductPriceService productPriceService = new ProductPriceService(_competitorPriceRepositoryMock.Object, _eventSenderServiceMock.Object, _productRepositoryMock.Object, _logerMock.Object, _productPriceOptions, _recommendationServiceMock.Object);

            var lastPrices = await productPriceService.GetLastPricesAsync(productId);

            _competitorPriceRepositoryMock.Verify(s => s.GetLastPricesAsync(productId, It.IsAny<string>(), _productPriceOptions.Value.HistoryPriceCount), Times.Exactly(Enum.GetValues<CompetitorIds>().Length));
            Assert.NotNull(lastPrices);
            Assert.NotNull(lastPrices.MinPrice);
            Assert.NotNull(lastPrices.MaxPrice);
            Assert.NotEmpty(lastPrices.Prices);
            Assert.Equal(competitorIds.Length, lastPrices.Prices.Count);
            Assert.Equal(competitorIds.Length * _productPriceOptions.Value.HistoryPriceCount, lastPrices.Prices.Select(e => e.Prices).Select(e => e.Count).Sum());
            Assert.Equal(minPrice.Price, lastPrices.MinPrice.Price);
            Assert.Equal(minPrice.CompetitorId, lastPrices.MinPrice.CompetitorId.ToString());
            Assert.Equal(maxPrice.Price, lastPrices.MaxPrice.Price);
            Assert.Equal(maxPrice.CompetitorId, lastPrices.MaxPrice.CompetitorId.ToString());
            _competitorPriceRepositoryMock.Reset();
        }
    }
}
