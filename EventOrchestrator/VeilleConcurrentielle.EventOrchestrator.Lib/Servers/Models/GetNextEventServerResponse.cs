namespace VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models
{
    public class GetNextEventServerResponse
    {
        public Event Event { get; set; }
        public List<EventSubscriber> Subscribers { get; set; }
        public List<EventConsumer> Consumers { get; set; }
    }
}
