extern alias mywebapp;

using Microsoft.Extensions.Logging;
using Moq;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Core.Services
{
    public class ProductPriceServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICompetitorPriceRepository> _competitorPriceRepositoryMock;
        private readonly Mock<IEventSenderService> _eventSenderServiceMock;
        private readonly Mock<ILogger<ProductPriceService>> _logerMock;

        public ProductPriceServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _competitorPriceRepositoryMock = new Mock<ICompetitorPriceRepository>();
            _eventSenderServiceMock = new Mock<IEventSenderService>();
            _logerMock = new Mock<ILogger<ProductPriceService>>();
        }

        [Fact]
        public async Task OnPriceIdentifedAsync_IfIsDifferentFromLastPrice_PushNewPrice()
        {
            string productId = "ProductId";
            string eventid = "PreviousEventid";

            _productRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<ProductEntity?>(new ProductEntity()
                                        {
                                            Id = productId
                                        }));
            _competitorPriceRepositoryMock.Setup(s => s.IsDifferntFromLastPriceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>()))
                                            .Returns(Task.FromResult(true));

            IProductPriceService productPriceService = new ProductPriceService(_competitorPriceRepositoryMock.Object, _eventSenderServiceMock.Object, _productRepositoryMock.Object, _logerMock.Object);
            await productPriceService.OnPriceIdentifedAsync(eventid, new PriceIdentifiedEventPayload()
            {
                ProductId = productId,
                RefererEventId = eventid
            });

            _productRepositoryMock.Verify(s => s.GetByIdAsync(productId), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.IsDifferntFromLastPriceAsync(productId, It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>()), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<CompetitorPriceEntity>()), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.GetMinPriceAsync(productId), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.GetMaxPriceAsync(productId), Times.Once());
            _eventSenderServiceMock.Verify(s => s.SendProductAddedOrUpdatedEvent(eventid, It.IsAny<ProductEntity>(), It.IsAny<ProductPrice>(), It.IsAny<ProductPrice>()), Times.Once());
        }

        [Fact]
        public async Task OnPriceIdentifedAsync_Abort_IfProductIdIsUnknown()
        {
            string productId = "ProductId";
            string eventid = "PreviousEventid";

            _productRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<ProductEntity?>(null));

            IProductPriceService productPriceService = new ProductPriceService(_competitorPriceRepositoryMock.Object, _eventSenderServiceMock.Object, _productRepositoryMock.Object, _logerMock.Object);
            await productPriceService.OnPriceIdentifedAsync(eventid, new PriceIdentifiedEventPayload()
            {
                ProductId = productId,
                RefererEventId = eventid
            });

            _productRepositoryMock.Verify(s => s.GetByIdAsync(productId), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.IsDifferntFromLastPriceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>()), Times.Never());
            _competitorPriceRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<CompetitorPriceEntity>()), Times.Never());
            _eventSenderServiceMock.Verify(s => s.SendProductAddedOrUpdatedEvent(eventid, It.IsAny<ProductEntity>(), It.IsAny<ProductPrice>(), It.IsAny<ProductPrice>()), Times.Never());
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
            _competitorPriceRepositoryMock.Setup(s => s.IsDifferntFromLastPriceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>()))
                                            .Returns(Task.FromResult(false));

            IProductPriceService productPriceService = new ProductPriceService(_competitorPriceRepositoryMock.Object, _eventSenderServiceMock.Object, _productRepositoryMock.Object, _logerMock.Object);
            await productPriceService.OnPriceIdentifedAsync(eventid, new PriceIdentifiedEventPayload()
            {
                ProductId = productId,
                RefererEventId = eventid
            });

            _productRepositoryMock.Verify(s => s.GetByIdAsync(productId), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.IsDifferntFromLastPriceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>()), Times.Once());
            _competitorPriceRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<CompetitorPriceEntity>()), Times.Never());
            _eventSenderServiceMock.Verify(s => s.SendProductAddedOrUpdatedEvent(eventid, It.IsAny<ProductEntity>(), It.IsAny<ProductPrice>(), It.IsAny<ProductPrice>()), Times.Never());
        }
    }
}
