using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Framework;

namespace VeilleConcurrentielle.Infrastructure.Core.Models.Events
{
    public class AddOrUpdateProductRequestedEvent : Event<AddOrUPdateProductRequestedEventPayload>
    {
        public override EventNames Name => EventNames.AddOrUpdateProductRequested;
    }

    public class AddOrUPdateProductRequestedEventPayload : EventPayload
    {
        public string? ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        [AtLeast(1)]
        public List<Strategy> Strategies { get; set; }
        [Required]
        [AtLeast(1)]
        public List<CompetitorConfig> CompetitorConfigs { get; set; }
        [Required]
        public string ShopProductId { get; set; }

        public class Strategy
        {
            [Required]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public StrategyIds Id { get; set; }
        }
        public class CompetitorConfig
        {
            [Required]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public CompetitorIds CompetitorId { get; set; }
            [Required]
            public ConfigHolder Holder { get; set; }
        }
    }
}
