using System.ComponentModel.DataAnnotations;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Infrastructure.Core.Data.Entities
{
    public class ReceivedEventEntity : EntityBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string SerializedPayload { get; set; }
        public DateTime DispatchedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Source { get; set; }
    }
}
