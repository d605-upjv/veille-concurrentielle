using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Entities
{
    public class RecommendationEntity : EntityBase
    {
        public string ProductId { get; set; }
        public double CurrentPrice { get; set; }
        public double Price { get; set; }
        public string StrategyId { get; set; }
        public DateTime CreatedAt { get; set; }

        public ProductEntity Product { get; set; }
    }
}
