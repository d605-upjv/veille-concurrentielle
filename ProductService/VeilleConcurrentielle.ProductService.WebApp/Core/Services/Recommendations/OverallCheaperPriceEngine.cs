using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services.Recommendations
{
    public class OverallCheaperPriceEngine : RecommendationEngine
    {
        public override StrategyIds StrategyId => StrategyIds.OverallCheaperPrice;

        public override double GetRecommendedPrice(double currentPrice, int quantity, CompetitorProductPrices lastCompetitorPrices)
        {
            var prices = lastCompetitorPrices.GetLatestPricesPerCompetitor().Select(e => e.Price).ToList();
            prices.Add(currentPrice);
            return prices.Min();
        }
    }
}
