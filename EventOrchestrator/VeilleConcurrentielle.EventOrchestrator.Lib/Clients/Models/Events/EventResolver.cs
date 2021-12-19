namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models.Events
{
    public static class EventResolver
    {
        public static Type GetEventType<TEventPayload>() where TEventPayload : EventPayload
        {
            if (typeof(TEventPayload) == typeof(NewPriceSubmittedEventPayload))
            {
                return typeof(NewPriceSubmittedEvent);
            }
            return null;
        }
    }
}
