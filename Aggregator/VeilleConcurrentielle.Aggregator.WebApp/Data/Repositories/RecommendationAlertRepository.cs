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
    }
}
