using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Entities
{
    public class ProductEntity : EntityBase
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<StrategyEntity> Strategies { get; set; }
        public ICollection<CompetitorConfigEntity> CompetitorConfigs { get; set; }
        public ICollection<CompetitorPriceEntity> CompetitorPrices { get; set; }
    }
}
