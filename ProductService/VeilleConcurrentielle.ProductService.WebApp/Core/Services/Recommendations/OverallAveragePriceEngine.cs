using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services.Recommendations
{
    public class OverallAveragePriceEngine : RecommendationEngine
    {
        public override StrategyIds StrategyId => StrategyIds.OverallAveragePrice;

        public override double GetRecommendedPrice(double currentPrice, int quantity, CompetitorProductPrices lastCompetitorPrices)
        {
            var prices = lastCompetitorPrices.GetLatestPricesPerCompetitor().Select(e => e.Price).ToList();
            prices.Add(currentPrice);
            return Math.Round(prices.Average(), 2);
        }
    }
}
