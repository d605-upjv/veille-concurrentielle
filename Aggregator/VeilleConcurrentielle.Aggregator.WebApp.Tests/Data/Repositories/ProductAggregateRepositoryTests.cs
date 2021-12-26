extern alias mywebapp;

using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Data.Repositories
{
    public class ProductAggregateRepositoryTests
    {
        [Fact]
        public void ComputeNewIdBeforeInsert_DoNotAlterId()
        {
            string id = "ProductId";
            ProductAggregateRepository productRepository = new ProductAggregateRepository(null);
            ProductAggregateEntity entity = new ProductAggregateEntity()
            {
                Id = id
            };
            productRepository.ComputeNewIdBeforeInsert(entity);

            Assert.Equal(id, entity.Id);
        }
    }
}
