using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities
{
    [Index(nameof(Name))]
    [Index(nameof(CreatedAt))]
    public class EventEntity : EntityBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string SerializedPayload { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Source { get; set; }
    }
}
