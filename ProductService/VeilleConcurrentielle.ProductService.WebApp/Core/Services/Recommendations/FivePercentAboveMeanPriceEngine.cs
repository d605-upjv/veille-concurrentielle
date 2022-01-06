using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services.Recommendations
{
    public class FivePercentAboveMeanPriceEngine : RecommendationEngine
    {
        public override StrategyIds StrategyId => StrategyIds.FivePercentAboveMeanPrice;

        public override double GetRecommendedPrice(double currentPrice, int quantity, CompetitorProductPrices lastCompetitorPrices)
        {
            var prices = lastCompetitorPrices.GetLatestPricesPerCompetitor().Select(e => e.Price).ToList();
            prices.Add(currentPrice);
            var average = Math.Round(prices.Average(), 2);
            return Math.Round((average * 1.05), 2);
        }
    }
}
