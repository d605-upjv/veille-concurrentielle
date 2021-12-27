using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class GetAllUnseenRecommendationAlertModels
    {
        public class GetAllUnseenRecommendationAlertResponse
        {
            public List<RecommendationAlert> Recommendations { get; set; }

            public class RecommendationAlert
            {
                public string Id { get; set; }
                public string ProductId { get; set; }
                public bool IsSeen { get; set; }
                public StrategyIds StrategyId { get; set; }
                public double CurrentPrice { get; set; }
                public double Price { get; set; }
                public DateTime CreatedAt { get; set; }
            }
        }
    }
}
