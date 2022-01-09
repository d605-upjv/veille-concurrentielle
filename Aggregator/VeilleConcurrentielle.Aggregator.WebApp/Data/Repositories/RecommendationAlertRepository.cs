using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories
{
    public class RecommendationAlertRepository : RepositoryBase<RecommendationAlertEntity>, IRecommendationAlertRepository
    {
        public RecommendationAlertRepository(AggregatorDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<RecommendationAlertEntity>> GetAllUnseenAsync()
        {
            AggregatorDbContext dbContext = (AggregatorDbContext)_dbContext;
            return await dbContext.RecommendationAlerts.AsNoTracking()
                                    .Where(e => e.IsSeen == false)
                                    .OrderByDescending(e => e.CreatedAt)
                                    .ToListAsync();
        }

        public async Task<int> GetAllUnseenCountAsync()
        {
            AggregatorDbContext dbContext = (AggregatorDbContext)_dbContext;
            return await dbContext.RecommendationAlerts.AsNoTracking()
                                    .Where(e => e.IsSeen == false)
                                    .CountAsync();
        }

        public async Task<List<(string ProductId, string ProductName, int Count)>> GetAllUnseenCountByProductAsync()
        {
            List<(string ProductId, string ProductName, int Count)> results = new List<(string ProductId, string ProductName, int Count)>();
            AggregatorDbContext dbContext = (AggregatorDbContext)_dbContext;
            var items = await dbContext.RecommendationAlerts.AsNoTracking()
                                    .Include(e => e.Product)
                                    .Where(e => e.IsSeen == false)
                                    .GroupBy(e => e.ProductId)
                                    .Select(e => new
                                    {
                                        ProductId = e.First().ProductId,
                                        ProductName = e.First().Product.Name,
                                        Count = e.Count()
                                    }).ToListAsync();
            return items.ConvertAll(e => (e.ProductId, e.ProductName, e.Count));
        }

        public async Task<int> SetRecommendationAlertsForProductToSeenAsync(string productId)
        {
            AggregatorDbContext dbContext = (AggregatorDbContext)_dbContext;
            var items = await dbContext.RecommendationAlerts.Where(e => e.ProductId == productId && e.IsSeen == false).ToArrayAsync();
            foreach (var item in items)
            {
                item.IsSeen = true;
                item.SeenAt = DateTime.Now;
            }
            dbContext.RecommendationAlerts.UpdateRange(items);
            await dbContext.SaveChangesAsync();
            return items.Count();
        }
    }
}
