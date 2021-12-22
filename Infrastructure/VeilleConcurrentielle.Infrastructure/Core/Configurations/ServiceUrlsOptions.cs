namespace VeilleConcurrentielle.Infrastructure.Core.Configurations
{
    public class ServiceUrlsOptions
    {
        public const string ServiceUrls = "ServiceUrls";
        public string AggregatorUrl { get; set; }
        public string EventUrl { get; set; }
        public string ProductUrl { get; set; }
    }
}
