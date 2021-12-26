extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Core.Services;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Core.Services
{
    public class ProductAggregateServiceTests
    {
        private readonly string _refererEventId = "SourceEventId";

        [Fact]
        public async Task StoreProduct_AddProduct_IfDosNotExistYet()
        {
            string productId = "ProductId";
            Mock<IProductAggregateRepository> productRepositoryMock = new Mock<IProductAggregateRepository>();
            IProductAggregateService productsService = new ProductAggregateService(productRepositoryMock.Object);

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
                CompetitorConfigs = new List<ProductAddedOrUpdatedEventPayload.ProductCompetitorConfig>()
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
            IProductAggregateService productsService = new ProductAggregateService(productRepositoryMock.Object);

            productRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<ProductAggregateEntity>()))
                                                .Callback<ProductAggregateEntity>(entity =>
                                                {
                                                    entity.Id = productId;
                                                });

            ProductAddedOrUpdatedEventPayload request = new ProductAddedOrUpdatedEventPayload()
            {
                ProductId = productId,
                Strategies = new List<ProductAddedOrUpdatedEventPayload.ProductStrategy>(),
                CompetitorConfigs = new List<ProductAddedOrUpdatedEventPayload.ProductCompetitorConfig>()
            };
            await productsService.StoreProductAsync(_refererEventId, request);

            Assert.Equal(productId, request.ProductId);
        }

        [Fact]
        public async Task StoreProduct_EditProduct_IfAlreadyExists()
        {
            string productId = "ProductId";
            Mock<IProductAggregateRepository> productRepositoryMock = new Mock<IProductAggregateRepository>();
            IProductAggregateService productsService = new ProductAggregateService(productRepositoryMock.Object);

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
                CompetitorConfigs = new List<ProductAddedOrUpdatedEventPayload.ProductCompetitorConfig>()
            };
            await productsService.StoreProductAsync(_refererEventId, request);

            productRepositoryMock.Verify(s => s.GetByIdAsync(It.IsAny<string>()), Times.Once());
            productRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<ProductAggregateEntity>()), Times.Never());
            productRepositoryMock.Verify(s => s.UpdateAsync(It.IsAny<ProductAggregateEntity>()), Times.Once());
        }
    }
}
