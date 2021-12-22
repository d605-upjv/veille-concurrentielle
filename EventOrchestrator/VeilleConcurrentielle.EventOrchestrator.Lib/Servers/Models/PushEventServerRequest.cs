using System.ComponentModel.DataAnnotations;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class PushEventServerRequest
    {
        [Required]
        public string EventName { get; set; }
        [Required]
        public string SerializedPayload { get; set; }
        [Required]
        public string Source { get; set; }
    }
}
