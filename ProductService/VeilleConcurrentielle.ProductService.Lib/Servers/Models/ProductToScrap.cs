using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.Lib.Servers.Models
{
    public class ProductToScrap
    {
        public string ProductId { get; set;}
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CompetitorIds CompetitorId { get; set; }
        public string ProductProfileUrl { get; set; }
    }
}
