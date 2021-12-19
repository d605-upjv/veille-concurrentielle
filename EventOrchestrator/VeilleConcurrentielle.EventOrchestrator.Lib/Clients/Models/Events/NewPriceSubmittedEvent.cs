using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models.Events
{
    public class NewPriceSubmittedEvent : Event<NewPriceSubmittedEventPayload>
    {
        public override EventNames Name => EventNames.NewPriceSubmitted;
    }

    public class NewPriceSubmittedEventPayload : EventPayload
    {
        public string ProductId { get; set; }
        public double Price { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PriceSources Source { get; set; }
    }

    public enum PriceSources
    {
        [EnumMember(Value = "NewPriceSubmitted")]
        PriceSubmissionApi
    }
}
