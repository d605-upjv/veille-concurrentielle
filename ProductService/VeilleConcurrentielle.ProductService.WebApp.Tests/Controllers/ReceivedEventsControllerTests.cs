extern alias mywebapp;

using Microsoft.Extensions.Logging;
using Moq;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Controllers;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data;
using System;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.Infrastructure.Core.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.Infrastructure.Tests.Core.Controllers;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Controllers
{
    public class ReceivedEventsControllerTests : ReceivedEventControllerTestsBase<ProductWebApp, mywebapp.Program, ProductDbContext>
    {
        private readonly Mock<ILogger<ReceivedEventsController>> _loggerMock;
        private readonly Mock<IReceivedEventRepository> _receivedEventRepositorMock;
        private readonly Mock<IEventServiceClient> _eventServiceClientMock;
        public ReceivedEventsControllerTests()
        {
            _loggerMock = new Mock<ILogger<ReceivedEventsController>>();
            _receivedEventRepositorMock = new Mock<IReceivedEventRepository>();
            _eventServiceClientMock = new Mock<IEventServiceClient>();
        }

        [Fact]
        public override Task ReceiveEvent_Integration()
        {
            return base.ReceiveEvent_Integration();
        }

        [Fact]
        public override Task ReceiveEvent_Integration_InvalidSerializedPayload_ReturnsInternalServerError()
        {
            return base.ReceiveEvent_Integration_InvalidSerializedPayload_ReturnsInternalServerError();
        }

        [Fact]
        public async Task ReceiveEvent_ProcessEvent_IsTriggered()
        {
            string eventId = "EventId";
            EventNames eventName = EventNames.Test;
            Mock<IEventProcessor> eventProcessorMock = new Mock<IEventProcessor>();
            ReceivedEventsController controller = new ReceivedEventsController(_receivedEventRepositorMock.Object, _loggerMock.Object, _eventServiceClientMock.Object, eventProcessorMock.Object);
            TestEventPayload testEventPayload = new TestEventPayload()
            {
                StringData = "String",
                RefererEventId = "PreviousEventId",
                IntData = 8
            };
            var serializedTestEventPayload = SerializationUtils.Serialize(testEventPayload);
            DispatchEventServerRequest request = new DispatchEventServerRequest()
            {
                ApplicationName = ApplicationNames.Aggregator,
                Source = EventSources.Test,
                CreatedAt = DateTime.Now.AddMinutes(-1),
                DispatchedAt = DateTime.Now,
                EventId = eventId,
                EventName = EventNames.Test,
                SerializedPayload = serializedTestEventPayload
            };
            var response = await controller.ReceiveEvent(request);
            eventProcessorMock.Verify(s => s.ProcessEventAsync(eventId, eventName, It.IsAny<EventPayload>()), Times.Once());
        }

        [Fact]
        public async Task ReceiveEvent_ProcessEvent_WhenFailed_CatchAndLog()
        {
            string eventId = "EventId";
            EventNames eventName = EventNames.Test;
            Mock<IEventProcessor> eventProcessorMock = new Mock<IEventProcessor>();
            ReceivedEventsController controller = new ReceivedEventsController(_receivedEventRepositorMock.Object, _loggerMock.Object, _eventServiceClientMock.Object, eventProcessorMock.Object);
            TestEventPayload testEventPayload = new TestEventPayload()
            {
                StringData = "String",
                RefererEventId = "PreviousEventId",
                IntData = 8
            };
            var serializedTestEventPayload = SerializationUtils.Serialize(testEventPayload);
            DispatchEventServerRequest request = new DispatchEventServerRequest()
            {
                ApplicationName = ApplicationNames.Aggregator,
                Source = EventSources.Test,
                CreatedAt = DateTime.Now.AddMinutes(-1),
                DispatchedAt = DateTime.Now,
                EventId = eventId,
                EventName = EventNames.Test,
                SerializedPayload = serializedTestEventPayload
            };
            Exception someException = new Exception("Gotcha");
            eventProcessorMock.Setup(s => s.ProcessEventAsync(eventId, eventName, It.IsAny<EventPayload>()))
                                    .Throws(someException);
            _loggerMock.Reset();

            var response = await controller.ReceiveEvent(request);

            eventProcessorMock.Verify(s => s.ProcessEventAsync(eventId, eventName, It.IsAny<EventPayload>()), Times.Once());
            _loggerMock.Verify(logger => logger.Log(
                                            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                                            It.Is<EventId>(eventId => eventId.Id == 0),
                                            It.Is<It.IsAnyType>((@object, @type) => true),
                                            It.IsAny<Exception>(),
                                            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                                        Times.Once);
        }
    }
}
