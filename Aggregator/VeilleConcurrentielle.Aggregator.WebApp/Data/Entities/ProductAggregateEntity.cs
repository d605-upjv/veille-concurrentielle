using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Entities
{
    public class ProductAggregateEntity : EntityBase
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public double? MinPrice { get; set; }
        public string? MinPriceCompetitorId { get; set; }
        public int? MinPriceQuantity { get; set; }
        public double? MaxPrice { get; set; }
        public string? MaxPriceCompetitorId { get; set; }
        public int? MaxPriceQuantit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<ProductAggregateStrategyEntity> Strategies { get; set; }
        public List<ProductAggregateCompetitorConfigEntity> CompetitorConfigs { get; set; }
        public List<ProductAggregatePriceEntity> LastPrices { get; set; }
        public List<ProductAggregateRecommendationEntity> Recommendations { get; set; }
    }
}
