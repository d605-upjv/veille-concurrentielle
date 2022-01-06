using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Entities
{
    public class ProductAggregatePriceEntity : EntityBase
    {
        public string ProductId { get; set; }
        public string CompetitorId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public ProductAggregateEntity Product { get; set; }
    }
}
