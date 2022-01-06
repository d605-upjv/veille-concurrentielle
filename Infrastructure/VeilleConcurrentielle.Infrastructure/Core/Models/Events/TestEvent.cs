namespace VeilleConcurrentielle.Infrastructure.Core.Models.Events
{
    public class TestEvent : Event<TestEventPayload>
    {
        public override EventNames Name => EventNames.Test;
    }

    public class TestEventPayload : EventPayload
    {
        public string StringData { get; set; }
        public int IntData { get; set; }
    }
}
