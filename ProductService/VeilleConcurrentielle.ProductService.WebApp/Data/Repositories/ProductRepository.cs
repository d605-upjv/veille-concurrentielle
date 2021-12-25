using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Data;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Repositories
{
    public class ProductRepository : RepositoryBase<ProductEntity>, IProductRepository
    {
        public ProductRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }

        public override void ComputeNewIdBeforeInsert(ProductEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
            }
        }

        public override async Task UpdateAsync(ProductEntity entity)
        {
            await base.UpdateAsync(entity);
            ProductDbContext dbContext = (ProductDbContext)_dbContext;
            var strategies = dbContext.Strategies.Where(e => e.ProductId == entity.Id).ToList();
            dbContext.Strategies.RemoveRange(strategies);
            var competitorConfigs = dbContext.CompetitorConfigs.Where(e => e.ProductId == entity.Id).ToList();
            dbContext.CompetitorConfigs.RemoveRange(competitorConfigs);
            await _dbContext.SaveChangesAsync();
            dbContext.Strategies.AddRange(entity.Strategies);
            dbContext.CompetitorConfigs.AddRange(entity.CompetitorConfigs);
            await _dbContext.SaveChangesAsync();
        }
    }
}
