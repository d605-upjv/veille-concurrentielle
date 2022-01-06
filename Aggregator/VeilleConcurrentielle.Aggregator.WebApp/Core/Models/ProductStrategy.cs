using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Models
{
    public class ProductStrategy
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StrategyIds StrategyId { get; set; }
        public string StrategyName { get; set; }
    }
}
