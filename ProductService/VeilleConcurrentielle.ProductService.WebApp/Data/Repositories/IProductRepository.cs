using VeilleConcurrentielle.Infrastructure.Data;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Repositories
{
    public interface IProductRepository : IRepository<ProductEntity>
    {
        Task<List<ProductEntity>> GetProductsToScrap();
    }
}
