using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities
{
    [Index(nameof(EventId))]
    public class EventConsumerEntity : EntityBase
    {
        [Required]
        public string EventId { get; set; }
        [Required]
        public string ApplicationName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Reason { get; set; }
    }
}
