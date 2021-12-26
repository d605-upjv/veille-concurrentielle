using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models.Events
{
    public class ProductAddedOrUpdatedEvent : Event<ProductAddedOrUpdatedEventPayload>
    {
        public override EventNames Name => EventNames.ProductAddedOrUpdated;
    }

    public class ProductAddedOrUpdatedEventPayload : EventPayload
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public ProductPrice? MinPrice { get; set; }
        public ProductPrice? MaxPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        public List<ProductStrategy> Strategies { get; set; }
        [Required]
        public List<ProductCompetitorConfig> CompetitorConfigs { get; set; }

        public class ProductStrategy
        {
            [Required]
            public string Id { get; set; }
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public StrategyIds StrategyId { get; set; }
            [Required]
            public string ProductId { get; set; }
        }

        public class ProductCompetitorConfig
        {
            [Required]
            public string Id { get; set; }
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public CompetitorIds CompetitorId { get; set; }
            [Required]
            public ConfigHolder Holder { get; set; }
        }
    }
}
