extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Models;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly Mock<IRecommendationService> _recommendationServiceMock;
        public ProductsServiceTests()
        {
            _productPriceServiceMock = new Mock<IProductPriceService>();
            _eventSenderServiceMock = new Mock<IEventSenderService>();
            _recommendationServiceMock = new Mock<IRecommendationService>();

            _productPriceServiceMock.Setup(s => s.GetLastPricesAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult(new CompetitorProductPrices()
                                        {
                                            Prices = new List<CompetitorProductPrices.CompetitorItemProductPrices>()
                                        }));
            _recommendationServiceMock.Setup(s => s.GetRecommendationsAsync(It.IsAny<GetRecommendationRequest>()))
                                        .Returns(Task.FromResult(new GetRecommendationResponse()
                                        {
                                            Recommendations = new List<ProductRecommendation>(),
                                            NewRecommendations = new List<ProductRecommendation>()
                                        }));
        }

        [Fact]
        public async Task OnAddOrUPdateProductRequestedAsync_AddProduct_IfIdIsNullOrEmpty()
        {
            string productId = "ProductId";
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            IProductsService productsService = new ProductsService(productRepositoryMock.Object, _eventSenderServiceMock.Object, _productPriceServiceMock.Object, _recommendationServiceMock.Object);

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
            IProductsService productsService = new ProductsService(productRepositoryMock.Object, _eventSenderServiceMock.Object, _productPriceServiceMock.Object, _recommendationServiceMock.Object);

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
            IProductsService productsService = new ProductsService(productRepositoryMock.Object, _eventSenderServiceMock.Object, _productPriceServiceMock.Object, _recommendationServiceMock.Object);

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
            _productPriceServiceMock.Verify(s => s.GetLastPricesAsync(productId), Times.Once());
            _eventSenderServiceMock.Verify(s => s.SendProductAddedOrUpdatedEvent(It.IsAny<string>(), It.IsAny<ProductEntity>(), It.IsAny<CompetitorProductPrices>(), It.IsAny<List<ProductRecommendation>>()), Times.Once());
            _eventSenderServiceMock.Verify(s => s.SendNewRecommendationPushedEvent(It.IsAny<string>(), productId, It.IsAny<List<ProductRecommendation>>()), Times.Once());
        }
    }
}
