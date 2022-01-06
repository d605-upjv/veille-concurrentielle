using System.Text.Json.Serialization;
using VeilleConcurrentielle.Aggregator.WebApp.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class GetProductToAddOrEditModels
    {
        public class GetProductToAddResponse
        {
            public List<ProductStrategy> AllStrategies { get; set; }
            public List<CompetitorConfig> CompetitorConfigs { get; set; }
            public List<ProductStrategy> SelectedStrategies { get; set; }
        }
        public class GetProductToEditResponse : GetProductToAddResponse
        {
            public MainShopProduct? MainShopProduct { get; set; }
        }

        public class CompetitorConfig
        {
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public CompetitorIds CompetitorId { get; set; }
            public string CompetitorName { get; set; }
            public string ProductUrl { get; set; }
            public string LogoUrl { get; set; }
        }
    }
}
