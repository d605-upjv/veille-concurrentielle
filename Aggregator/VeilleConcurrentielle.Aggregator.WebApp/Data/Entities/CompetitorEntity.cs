using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Entities
{
    public class CompetitorEntity : EntityBase
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
    }
}
