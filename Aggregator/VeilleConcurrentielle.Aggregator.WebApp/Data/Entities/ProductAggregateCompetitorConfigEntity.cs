using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Entities
{
    public class ProductAggregateCompetitorConfigEntity : EntityBase
    {
        public string ProductId { get; set; }
        public string CompetitorId { get; set; }
        public string SerializedHolder { get; set; }

        public ProductAggregateEntity Product { get; set; }
    }
}
