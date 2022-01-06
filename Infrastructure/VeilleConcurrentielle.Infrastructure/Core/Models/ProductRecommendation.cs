using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public class ProductRecommendation
    {
        [Required]
        public string Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StrategyIds StrategyId { get; set; }
        public double CurrentPrice { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
