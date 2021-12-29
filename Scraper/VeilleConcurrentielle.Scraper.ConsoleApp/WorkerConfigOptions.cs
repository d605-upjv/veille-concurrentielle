using System.Text.Json.Serialization;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.Scraper.ConsoleApp
{
    public class WorkerConfigOptions
    {
        public const string WorkerConfig = "WorkerConfig";
        public bool InfiniteRun { get; set; } = false;
        public int NextRoundWaitTimeInSeconds { get; set; } = 150;
        public int MaxParallelCount { get; set; } = 5;
        public List<ShopConfig> ShopConfigs { get; set; }
    }

    public class ShopConfig
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CompetitorIds CompetitorId { get; set; }
        public string XPath { get; set; }
        public int DefaultQuantity { get; set; } = 10;
    }
}
