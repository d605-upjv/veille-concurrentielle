using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Models
{
    public class GetRecommendationRequest
    {
        public string ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public List<StrategyIds> Strategies { get; set; }
        public CompetitorProductPrices LastCompetitorPrices { get; set; }
    }
}
