extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Core.Services
{
    public class EventProcessorTests
    {
        [Fact]
        public async Task ProcessEventAsync_AddOrUpdateProductRequested()
        {
            string eventId = "EventId";
            EventNames eventName = EventNames.AddOrUpdateProductRequested;
            var payload = new AddOrUPdateProductRequestedEventPayload();
            Mock<IProductsService> productServiceMock = new Mock<IProductsService>();
            IEventProcessor eventProcessor = new EventProcessor(productServiceMock.Object);

            await eventProcessor.ProcessEventAsync(eventId, eventName, payload);
            productServiceMock.Verify(s => s.StoreProductAsync(eventId, payload), Times.Once());
        }
    }
}
