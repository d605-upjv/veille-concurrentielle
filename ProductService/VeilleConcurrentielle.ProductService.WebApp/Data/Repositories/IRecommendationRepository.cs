using VeilleConcurrentielle.Infrastructure.Data;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Repositories
{
    public interface IRecommendationRepository : IRepository<RecommendationEntity>
    {
        Task<RecommendationEntity?> GetLatestRecommendationAsync(string productId, string strategyId);
    }
}
