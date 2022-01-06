using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Models
{
    public class ProductRecommendation
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StrategyIds StrategyId { get; set; }
        public string StrategyName { get; set; }
        public double Price { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
