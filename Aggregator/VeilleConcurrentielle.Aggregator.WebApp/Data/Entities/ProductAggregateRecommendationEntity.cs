using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Entities
{
    public class ProductAggregateRecommendationEntity : EntityBase
    {
        public string ProductId { get; set; }
        public string StrategyId { get; set; }
        public double Price { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        public ProductAggregateEntity Product { get; set; }
    }
}
