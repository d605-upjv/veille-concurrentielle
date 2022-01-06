namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public abstract class EventPayload
    {
        public string? RefererEventId { get; set; }
    }
}
