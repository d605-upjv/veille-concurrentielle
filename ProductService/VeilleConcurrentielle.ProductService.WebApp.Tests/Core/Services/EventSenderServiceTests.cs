extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Core.Services
{
    public class EventSenderServiceTests
    {
        private readonly Mock<IEventServiceClient> _eventServiceClientMock;
        public EventSenderServiceTests()
        {
            _eventServiceClientMock = new Mock<IEventServiceClient>();
        }

        [Fact]
        public async Task SendProductAddedOrUpdatedEvent_CheckServiceCall()
        {
            IEventSenderService eventSenderService = new EventSenderService(_eventServiceClientMock.Object);
            ProductEntity productEntity = new ProductEntity()
            {
                Strategies = new List<StrategyEntity>(),
                CompetitorConfigs = new List<CompetitorConfigEntity>()
            };
            await eventSenderService.SendProductAddedOrUpdatedEvent("eventId", productEntity, new CompetitorProductPrices(), new List<ProductRecommendation>());
            _eventServiceClientMock.Verify(s => s.PushEventAsync(It.IsAny<PushEventClientRequest<ProductAddedOrUpdatedEvent, ProductAddedOrUpdatedEventPayload>>()), Times.Once());
        }

        [Fact]
        public async Task SendNewRecommendationPushedEvent_NoCallIfNoRecommendations()
        {
            IEventSenderService eventSenderService = new EventSenderService(_eventServiceClientMock.Object);
            await eventSenderService.SendNewRecommendationPushedEvent("eventId", "productId", new List<ProductRecommendation>());
            _eventServiceClientMock.Verify(s => s.PushEventAsync(It.IsAny<PushEventClientRequest<NewRecommendationPushedEvent, NewRecommendationPushedEventPayload>>()), Times.Never());
        }

        [Fact]
        public async Task SendNewRecommendationPushedEvent_AsMuchCallsAsRecommendations()
        {
            IEventSenderService eventSenderService = new EventSenderService(_eventServiceClientMock.Object);
            List<ProductRecommendation> newRecommendations = new List<ProductRecommendation>();
            newRecommendations.Add(new ProductRecommendation());
            newRecommendations.Add(new ProductRecommendation());
            await eventSenderService.SendNewRecommendationPushedEvent("eventId", "productId", newRecommendations);
            _eventServiceClientMock.Verify(s => s.PushEventAsync(It.IsAny<PushEventClientRequest<NewRecommendationPushedEvent, NewRecommendationPushedEventPayload>>()), Times.Exactly(newRecommendations.Count));
        }
    }
}
