using System.ComponentModel.DataAnnotations;

namespace VeilleConcurrentielle.Infrastructure.Core.Models.Events
{
    public class ComputeRecommendationRequestedEvent
    {
    }

    public class ComputeRecommendationRequestedEventPayload : EventPayload
    {
        [Required]
        public string ProductId { get; set; }
    }
}
