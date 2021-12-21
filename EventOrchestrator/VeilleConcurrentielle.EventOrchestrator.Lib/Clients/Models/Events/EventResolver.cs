namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models.Events
{
    public static class EventResolver
    {
        private static Dictionary<Type, Type> _registry = new Dictionary<Type, Type>()
        {
            {typeof(NewPriceSubmittedEventPayload), typeof(NewPriceSubmittedEvent) },
            {typeof(AddOrUPdateProductRequestedEventPayload), typeof(AddOrUpdateProductRequestedEvent) }
        };
        public static Type GetEventType<TEventPayload>() where TEventPayload : EventPayload
        {
            return _registry[typeof(TEventPayload)];
        }
    }
}
