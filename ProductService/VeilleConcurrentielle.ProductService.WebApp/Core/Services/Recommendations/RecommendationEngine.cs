using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services.Recommendations
{
    public abstract class RecommendationEngine
    {
        public abstract StrategyIds StrategyId { get; }
        public abstract double GetRecommendedPrice(double currentPrice, int quantity, CompetitorProductPrices lastCompetitorPrices);
    }
}
