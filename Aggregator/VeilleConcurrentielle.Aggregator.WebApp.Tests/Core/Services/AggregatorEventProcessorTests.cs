extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Core.Services;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Core.Services;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Core.Services
{
    public class AggregatorEventProcessorTests
    {
        [Fact]
        public async Task ProcessEventAsync_ProductAddedOrUpdated()
        {
            string eventId = "EventId";
            EventNames eventName = EventNames.ProductAddedOrUpdated;
            var payload = new ProductAddedOrUpdatedEventPayload();
            Mock<IProductAggregateService> productServiceMock = new Mock<IProductAggregateService>();
            IEventProcessor eventProcessor = new AggregatorEventProcessor(productServiceMock.Object);

            await eventProcessor.ProcessEventAsync(eventId, eventName, payload);
            productServiceMock.Verify(s => s.StoreProductAsync(eventId, payload), Times.Once());
        }
    }
}
