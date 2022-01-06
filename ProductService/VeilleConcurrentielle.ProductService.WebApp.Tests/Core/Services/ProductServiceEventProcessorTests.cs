extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Core.Services;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Core.Services
{
    public class ProductServiceEventProcessorTests
    {
        private Mock<IProductsService> _productServiceMock;
        private Mock<IProductPriceService> _productPriceServiceMock;
        private IEventProcessor _eventProcessor;
        public ProductServiceEventProcessorTests()
        {
            _productServiceMock = new Mock<IProductsService>();
            _productPriceServiceMock = new Mock<IProductPriceService>();
            _eventProcessor = new ProductServiceEventProcessor(_productServiceMock.Object, _productPriceServiceMock.Object);
        }
        [Fact]
        public async Task ProcessEventAsync_AddOrUpdateProductRequested()
        {
            string eventId = "EventId";
            EventNames eventName = EventNames.AddOrUpdateProductRequested;
            var payload = new AddOrUPdateProductRequestedEventPayload();

            await _eventProcessor.ProcessEventAsync(eventId, eventName, payload);
            _productServiceMock.Verify(s => s.OnAddOrUPdateProductRequestedAsync(eventId, payload), Times.Once());
        }

        [Fact]
        public async Task ProcessEventAsync_PriceIdentified()
        {
            string eventId = "EventId";
            EventNames eventName = EventNames.PriceIdentified;
            var payload = new  PriceIdentifiedEventPayload();

            await _eventProcessor.ProcessEventAsync(eventId, eventName, payload);
            _productPriceServiceMock.Verify(s => s.OnPriceIdentifedAsync(eventId, payload), Times.Once());
        }
    }
}
