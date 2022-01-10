extern alias mywebapp;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.TestLib.TestData;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Data.Repositories
{
    public class ProductRepositoryTests
    {
        [Fact]
        public void ComputeNewIdBeforeInsert_KeepIdIfNotEmpty()
        {
            string productId = "productId";
            IProductRepository productRepository = new ProductRepository(null);

            ProductEntity productEntity = new ProductEntity();
            productEntity.Id = productId;
            ((ProductRepository)productRepository).ComputeNewIdBeforeInsert(productEntity);

            Assert.Equal(productId, productEntity.Id);
        }

        [Theory]
        [ClassData(typeof(EmptyStringTestData))]
        public void ComputeNewIdBeforeInsert_GenerateNewIfEmpty(string productId)
        {
            IProductRepository productRepository = new ProductRepository(null);

            ProductEntity productEntity = new ProductEntity();
            productEntity.Id = productId;
            ((ProductRepository)productRepository).ComputeNewIdBeforeInsert(productEntity);

            Assert.False(string.IsNullOrWhiteSpace(productEntity.Id));
        }
    }
}
