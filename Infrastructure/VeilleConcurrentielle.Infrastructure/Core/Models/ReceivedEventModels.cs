using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public class ReceivedEventModels
    {
        public class ReceivedEentRequest
        {
            [JsonConverter(typeof(JsonStringEnumConverter))]
            [Required]
            public EventNames EventName { get; set; }
            [Required]
            public string SerializedPayload { get; set; }
            [Required]
            public DateTime SubmittedAt { get; set; }
            [Required]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public EventSources Source { get; set; }
        }

        public class ReceivedEentResponse
        {
            public string Id { get; set; }
        }
    }
}
