using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.ProductService.WebApp.Core.Models;
using VeilleConcurrentielle.ProductService.WebApp.Core.Services.Recommendations;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public interface IRecommendationService
    {
        Task<GetRecommendationResponse> GetRecommendationsAsync(GetRecommendationRequest request);
        RecommendationEngine GetRecommendationEngine(StrategyIds strategyId);
    }
}