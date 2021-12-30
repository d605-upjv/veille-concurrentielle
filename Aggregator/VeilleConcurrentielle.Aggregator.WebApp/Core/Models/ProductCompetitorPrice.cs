using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Models
{
    public class ProductCompetitorPrice
    {
        public CompetitorIds CompetitorId { get; set; }
        public string CompetitorName { get; set; }
        public string CompetitorLogUrl { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
