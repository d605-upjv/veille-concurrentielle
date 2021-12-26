using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models.Events
{
    public class PriceIdentifiedEvent : Event<PriceIdentifiedEventPayload>
    {
        public override EventNames Name => EventNames.PriceIdentified;
    }

    public class PriceIdentifiedEventPayload : EventPayload
    {
        [Required]
        public string ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CompetitorIds CompetitorId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PriceSources Source { get; set; }
    }

    public enum PriceSources
    {
        [EnumMember(Value = "PriceApi")]
        PriceApi,
        [EnumMember(Value = "PriceCrawler")]
        PriceCrawler
    }
}
