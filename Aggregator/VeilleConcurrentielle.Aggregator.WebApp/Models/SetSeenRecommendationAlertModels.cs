using System.ComponentModel.DataAnnotations;

namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class SetSeenRecommendationAlertModels
    {
        public class SetSeenRecommendationAlertResponse
        {
            public DateTime SeenAt { get; set; }
        }
    }
}
