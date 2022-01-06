using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Entities
{
    [Index(nameof(IsSeen))]
    public class RecommendationAlertEntity : EntityBase
    {
        public string ProductId { get; set; }
        public bool IsSeen { get; set; }
        public string StrategyId { get; set; }
        public double CurrentPrice { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SeenAt { get; set; }

        public ProductAggregateEntity Product { get; set; }
    }
}
