using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Entities
{
    public class ProductAggregateStrategyEntity : EntityBase
    {
        public string ProductId { get; set; }
        public string StrategyId { get; set; }

        public ProductAggregateEntity Product { get; set; }
    }
}
