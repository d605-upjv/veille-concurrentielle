using VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories
{
    public interface IRecommendationAlertRepository : IRepository<RecommendationAlertEntity>
    {
        Task<List<RecommendationAlertEntity>> GetAllUnseenAsync();
        Task<int> GetAllUnseenCountAsync();
        Task<int> SetRecommendationAlertsForProductToSeenAsync(string productId);
        Task<List<(string ProductId, string ProductName, int Count)>> GetAllUnseenCountByProductAsync();
    }
}
