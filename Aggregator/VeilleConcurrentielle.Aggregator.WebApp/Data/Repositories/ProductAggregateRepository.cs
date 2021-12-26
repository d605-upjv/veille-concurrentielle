using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories
{
    public class ProductAggregateRepository : RepositoryBase<ProductAggregateEntity>, IProductAggregateRepository
    {
        public ProductAggregateRepository(AggregatorDbContext dbContext) : base(dbContext)
        {
        }

        public override void ComputeNewIdBeforeInsert(ProductAggregateEntity entity)
        {
            
        }

        public override async Task UpdateAsync(ProductAggregateEntity entity)
        {
            await base.UpdateAsync(entity);
            AggregatorDbContext dbContext = (AggregatorDbContext)_dbContext;
            var strategies = dbContext.ProductAggregateStrategies.Where(e => e.ProductId == entity.Id).ToList();
            dbContext.ProductAggregateStrategies.RemoveRange(strategies);
            var competitorConfigs = dbContext.ProductAggregateCompetitorConfigs.Where(e => e.ProductId == entity.Id).ToList();
            dbContext.ProductAggregateCompetitorConfigs.RemoveRange(competitorConfigs);
            var lastPrices = dbContext.ProductAggregatePrices.Where(e => e.ProductId == entity.Id).ToList();
            dbContext.ProductAggregatePrices.RemoveRange(lastPrices);
            await _dbContext.SaveChangesAsync();
            dbContext.ProductAggregateStrategies.AddRange(entity.Strategies);
            dbContext.ProductAggregateCompetitorConfigs.AddRange(entity.CompetitorConfigs);
            dbContext.ProductAggregatePrices.AddRange(entity.LastPrices);
            await _dbContext.SaveChangesAsync();
        }
    }
}
