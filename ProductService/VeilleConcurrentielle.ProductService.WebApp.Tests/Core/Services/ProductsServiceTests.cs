extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Core.Services
{
    public class ProductsServiceTests
    {
        private readonly string _refererEventId = "SourceEventId";
        private readonly Mock<IProductPriceService> _productPriceServiceMock;
        private readonly Mock<IEventSenderService> _eventSenderServiceMock;
        public ProductsServiceTests()
        {
            _productPriceServiceMock = new Mock<IProductPriceService>();
            _eventSenderServiceMock = new Mock<IEventSenderService>();

            _productPriceServiceMock.Setup(s => s.GetMinPriceAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<ProductPrice?>(null));
            _productPriceServiceMock.Setup(s => s.GetMaxPriceAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<ProductPrice?>(null));
        }

        [Fact]
        public async Task OnAddOrUPdateProductRequestedAsync_AddProduct_IfIdIsNullOrEmpty()
        {
            string productId = "ProductId";
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            IProductsService productsService = new ProductsService(productRepositoryMock.Object, _eventSenderServiceMock.Object, _productPriceServiceMock.Object);

            productRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<ProductEntity>()))
                                                .Callback<ProductEntity>(entity =>
                                                {
                                                    entity.Id = productId;
                                                });
            _productPriceServiceMock.Reset();

            AddOrUPdateProductRequestedEventPayload request = new AddOrUPdateProductRequestedEventPayload()
            {
                ProductId = null,
                Strategies = new List<AddOrUPdateProductRequestedEventPayload.Strategy>(),
                CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>()
            };
            await productsService.OnAddOrUPdateProductRequestedAsync(_refererEventId, request);

            productRepositoryMock.Verify(s => s.GetByIdAsync(It.IsAny<string>()), Times.Never());
            productRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<ProductEntity>()), Times.Once());
            productRepositoryMock.Verify(s => s.UpdateAsync(It.IsAny<ProductEntity>()), Times.Never());
            CheckCommonServicesForStoreProduct(productId);
        }

        [Fact]
        public async Task OnAddOrUPdateProductRequestedAsync_AddProduct_KeepProvidedProductId()
        {
            string productId = "ProductId";
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            IProductsService productsService = new ProductsService(productRepositoryMock.Object, _eventSenderServiceMock.Object, _productPriceServiceMock.Object);

            productRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<ProductEntity>()))
                                                .Callback<ProductEntity>(entity =>
                                                {
                                                    entity.Id = productId;
                                                });
            _productPriceServiceMock.Reset();

            AddOrUPdateProductRequestedEventPayload request = new AddOrUPdateProductRequestedEventPayload()
            {
                ProductId = productId,
                Strategies = new List<AddOrUPdateProductRequestedEventPayload.Strategy>(),
                CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>()
            };
            await productsService.OnAddOrUPdateProductRequestedAsync(_refererEventId, request);

            Assert.Equal(productId, request.ProductId);
        }

        [Fact]
        public async Task OnAddOrUPdateProductRequestedAsync_EditProduct_IfIdIsProvided()
        {
            string productId = "ProductId";
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            IProductsService productsService = new ProductsService(productRepositoryMock.Object, _eventSenderServiceMock.Object, _productPriceServiceMock.Object);

            productRepositoryMock.Setup(s => s.GetByIdAsync(productId))
                                            .Returns(Task.FromResult<ProductEntity?>(
                                                new ProductEntity()
                                                {
                                                    Id = productId
                                                }
                                            ));
            _productPriceServiceMock.Reset();

            AddOrUPdateProductRequestedEventPayload request = new AddOrUPdateProductRequestedEventPayload()
            {
                ProductId = productId,
                Strategies = new List<AddOrUPdateProductRequestedEventPayload.Strategy>(),
                CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>()
            };
            await productsService.OnAddOrUPdateProductRequestedAsync(_refererEventId, request);

            productRepositoryMock.Verify(s => s.GetByIdAsync(It.IsAny<string>()), Times.Once());
            productRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<ProductEntity>()), Times.Never());
            productRepositoryMock.Verify(s => s.UpdateAsync(It.IsAny<ProductEntity>()), Times.Once());
            CheckCommonServicesForStoreProduct(productId);
        }

        private void CheckCommonServicesForStoreProduct(string productId)
        {
            _productPriceServiceMock.Verify(s => s.GetMinPriceAsync(productId), Times.Once());
            _productPriceServiceMock.Verify(s => s.GetMaxPriceAsync(productId), Times.Once());
            _eventSenderServiceMock.Verify(s => s.SendProductAddedOrUpdatedEvent(It.IsAny<string>(), It.IsAny<ProductEntity>(), It.IsAny<ProductPrice?>(), It.IsAny<ProductPrice?>()), Times.Once());
        }
    }
}
