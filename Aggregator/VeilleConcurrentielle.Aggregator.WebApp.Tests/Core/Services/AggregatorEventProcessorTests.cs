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
        private readonly Mock<IProductAggregateService> _productServiceMock;
        private readonly Mock<IRecommendationAlertService> _recommendationAlertServiceMock;

        public AggregatorEventProcessorTests()
        {
            _productServiceMock = new Mock<IProductAggregateService>();
            _recommendationAlertServiceMock = new Mock<IRecommendationAlertService>();
        }

        [Fact]
        public async Task ProcessEventAsync_ProductAddedOrUpdated()
        {
            string eventId = "EventId";
            EventNames eventName = EventNames.ProductAddedOrUpdated;
            var payload = new ProductAddedOrUpdatedEventPayload();
            IEventProcessor eventProcessor = new AggregatorEventProcessor(_productServiceMock.Object, _recommendationAlertServiceMock.Object);

            await eventProcessor.ProcessEventAsync(eventId, eventName, payload);
            _productServiceMock.Verify(s => s.StoreProductAsync(eventId, payload), Times.Once());
        }

        [Fact]
        public async Task ProcessEventAsync_NewRecommendationPushed()
        {
            string eventId = "EventId";
            EventNames eventName = EventNames.NewRecommendationPushed;
            var payload = new NewRecommendationPushedEventPayload();
            IEventProcessor eventProcessor = new AggregatorEventProcessor(_productServiceMock.Object, _recommendationAlertServiceMock.Object);

            await eventProcessor.ProcessEventAsync(eventId, eventName, payload);
            _recommendationAlertServiceMock.Verify(s => s.StoreNewRecommendationAlertAsync(eventId, payload), Times.Once());
        }
    }
}
