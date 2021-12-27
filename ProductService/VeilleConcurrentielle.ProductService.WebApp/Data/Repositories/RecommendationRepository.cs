using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Data;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Repositories
{
    public class RecommendationRepository : RepositoryBase<RecommendationEntity>, IRecommendationRepository
    {
        public RecommendationRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<RecommendationEntity?> GetLatestRecommendationAsync(string productId, string strategyId)
        {
            ProductDbContext dbContext = (ProductDbContext)_dbContext;
            return await dbContext.Recommendations.FirstOrDefaultAsync(e => e.ProductId == productId && e.StrategyId == strategyId);
        }
    }
}
