using System;
using VeilleConcurrentielle.Infrastructure.Core.Framework;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using Xunit;

namespace VeilleConcurrentielle.Infrastructure.Tests.Core.Framework
{
    public class EventResolverTests
    {
        [Fact]
        public void GetEventPayloadType_AllEventNames()
        {
            var eventNames = Enum.GetValues<EventNames>();
            foreach(var eventName in eventNames)
            {
                var eventPayloadType = EventResolver.GetEventPayloadType(eventName);
                Assert.NotNull(eventPayloadType);
                Assert.True(eventPayloadType.IsSubclassOf(typeof(EventPayload)));
            }
        }

        [Fact]
        public void GetEventType_AllEvents()
        {
            var eventNames = Enum.GetValues<EventNames>();
            foreach(var eventName in eventNames)
            {
                var eventPayloadType = EventResolver.GetEventPayloadType(eventName);
                var eventType = EventResolver.GetEventType(eventPayloadType);
                Assert.NotNull(eventType);
            }
        }
    }
}
