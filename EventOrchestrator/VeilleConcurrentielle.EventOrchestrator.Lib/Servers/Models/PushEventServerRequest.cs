namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class PushEventServerRequest
    {
        public string EventName { get; set; }
        public string SerializedPayload { get; set; }
        public string Source { get; set; }
    }
}
