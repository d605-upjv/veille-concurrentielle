using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;

namespace VeilleConcurrentielle.Infrastructure.Core.Framework
{
    public static class EventResolver
    {
        private static Dictionary<Type, Type> _eventByPayloadRegistry = new Dictionary<Type, Type>()
        {
            {typeof(TestEventPayload), typeof(TestEvent) },
            {typeof(NewPriceSubmittedEventPayload), typeof(NewPriceSubmittedEvent) },
            {typeof(AddOrUPdateProductRequestedEventPayload), typeof(AddOrUpdateProductRequestedEvent) },
            {typeof(ProductAddedOrUpdatedEventPayload), typeof(ProductAddedOrUpdatedEvent) }
        };
        private static Dictionary<EventNames, Type> _eventPayloadByNameRegistry = new Dictionary<EventNames, Type>()
        {
            { EventNames.Test, typeof(TestEventPayload) },
            { EventNames.NewPriceSubmitted, typeof(NewPriceSubmittedEventPayload) },
            { EventNames.AddOrUpdateProductRequested, typeof(AddOrUPdateProductRequestedEventPayload) },
            { EventNames.ProductAddedOrUpdated, typeof(ProductAddedOrUpdatedEventPayload) }
        };
        public static Type GetEventType<TEventPayload>() where TEventPayload : EventPayload
        {
            return GetEventType(typeof(TEventPayload));
        }
        public static Type GetEventType(Type eventPayloadType)
        {
            return _eventByPayloadRegistry[eventPayloadType];
        }
        public static Type GetEventPayloadType(EventNames eventName)
        {
            return _eventPayloadByNameRegistry[eventName];
        }
    }
}
