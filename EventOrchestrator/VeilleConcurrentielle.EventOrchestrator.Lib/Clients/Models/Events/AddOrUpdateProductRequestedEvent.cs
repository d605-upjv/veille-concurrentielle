using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Framework;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models.Events
{
    public class AddOrUpdateProductRequestedEvent : Event<AddOrUPdateProductRequestedEventPayload>
    {
        public override EventNames Name => EventNames.AddOrUpdateProductRequested;
    }

    public class AddOrUPdateProductRequestedEventPayload : EventPayload
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [AtLeast(1)]
        public List<Strategy> Strategys { get; set; }
        [Required]
        public List<CompetitorConfig> CompetitorConfigs { get; set; }

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
            public List<ProductExternalId> ProductExternalIds { get; set; }
            [Required]
            public List<ProductUrl> ProductUrls { get; set; }

            public class ProductExternalId
            {
                [Required]
                [JsonConverter(typeof(JsonStringEnumConverter))]
                public ProductExternalIdNames Name { get; set; }
                [Required]
                public string Id { get; set; }
            }

            public class ProductUrl
            {
                [Required]
                [JsonConverter(typeof(JsonStringEnumConverter))]
                public ProductUrlNames Name { get; set; }
                [Required]
                public string Url { get; set; }
            }
        }
    }

    public enum CompetitorIds
    {
        [EnumMember(Value = "ShopA")]
        ShopA,
        [EnumMember(Value = "ShopB")]
        ShopB
    }

    public enum StrategyIds
    {
        [EnumMember(Value = "OverallAveragePrice")]
        OverallAveragePrice,
        [EnumMember(Value = "OverallCheaperPrice")]
        OverallCheaperPrice,
        [EnumMember(Value = "FivePercentAboveMeanPrice")]
        FivePercentAboveMeanPrice
    }

    public enum ProductExternalIdNames
    {
        [EnumMember(Value = "UniqueId")]
        UniqueId,
        [EnumMember(Value = "EAN")]
        EAN
    }

    public enum ProductUrlNames
    {
        [EnumMember(Value = "ProductProfile")]
        ProductProfile,
    }
}
