namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Source { get; set; }
        public string SerializedPayload { get; set; }
    }
}
