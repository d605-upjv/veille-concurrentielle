using System.ComponentModel.DataAnnotations;

namespace VeilleConcurrentielle.Infrastructure.Core.Models.Events
{
    public class NewRecommendationPushedEvent : Event<NewRecommendationPushedEventPayload>
    {
        public override EventNames Name => EventNames.NewRecommendationPushed;
    }

    public class NewRecommendationPushedEventPayload : EventPayload
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public ProductRecommendation Recommendation { get; set; }
    }
}
