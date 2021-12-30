using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Models
{
    public class ProductCompetitorConfig
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CompetitorIds CompetitorId { get; set; }
        public ConfigHolder Holder { get; set; }
    }
}
