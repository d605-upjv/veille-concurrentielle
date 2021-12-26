using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.EventOrchestrator.Lib.Controllers;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.Infrastructure.Core.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Core.Services;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.Infrastructure.TestLib;
using VeilleConcurrentielle.Infrastructure.Web;
using Xunit;

namespace VeilleConcurrentielle.Infrastructure.Tests.Core.Controllers
{
    public abstract class ReceivedEventControllerTestsBase<TWebApp, TEntryPoint, TDbContext, TReceivedEventController>
        where TWebApp : WebAppBase<TEntryPoint, TDbContext>, new()
        where TEntryPoint : class
        where TDbContext : DbContext
        where TReceivedEventController: ApiControllerBase, IReceivedEventsController
    {
        protected readonly Mock<ILogger<TReceivedEventController>> _loggerMock;
        protected readonly Mock<IReceivedEventRepository> _receivedEventRepositorMock;
        protected readonly Mock<IEventServiceClient> _eventServiceClientMock;
        protected readonly Mock<IEventProcessor> _eventProcessorMock;

        public ReceivedEventControllerTestsBase()
        {
            _loggerMock = new Mock<ILogger<TReceivedEventController>>();
            _receivedEventRepositorMock = new Mock<IReceivedEventRepository>();
            _eventServiceClientMock = new Mock<IEventServiceClient>();
            _eventProcessorMock = new Mock<IEventProcessor>();
        }

        protected abstract TReceivedEventController CreateController();

        public virtual async Task ReceiveEvent_Integration()
        {
            await using var application = new TWebApp();
            using var client = application.CreateClient();
            var payload = new TestEventPayload()
            {
                StringData = "String",
                IntData = 10
            };
            var serializedPayload = SerializationUtils.Serialize(payload);
            DispatchEventServerRequest request = new DispatchEventServerRequest()
            {
                EventId = "EventId",
                EventName = EventNames.Test,
                Source = EventSources.Test,
                DispatchedAt = DateTime.Now,
                SerializedPayload = serializedPayload
            };
            var response = await client.PostAsJsonAsync("/api/ReceivedEvents", request);
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<DispatchEventServerResponse>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent.ReceivedEventId);
        }

        public virtual async Task ReceiveEvent_Integration_InvalidSerializedPayload_ReturnsInternalServerError()
        {
            await using var application = new TWebApp();
            using var client = application.CreateClient();
            DispatchEventServerRequest request = new DispatchEventServerRequest()
            {
                EventId = "EventId",
                EventName = EventNames.PriceIdentified,
                Source = EventSources.Test,
                DispatchedAt = DateTime.Now,
                SerializedPayload = "invalid serialized payload"
            };
            var response = await client.PostAsJsonAsync("/api/ReceivedEvents", request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        public virtual async Task ReceiveEvent_ProcessEvent_IsTriggered()
        {
            string eventId = "EventId";
            EventNames eventName = EventNames.Test;
            TReceivedEventController controller = CreateController();
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
            _eventProcessorMock.Verify(s => s.ProcessEventAsync(eventId, eventName, It.IsAny<EventPayload>()), Times.Once());
        }

        public virtual async Task ReceiveEvent_ProcessEvent_WhenFailed_CatchAndLog()
        {
            string eventId = "EventId";
            EventNames eventName = EventNames.Test;
            TReceivedEventController controller = CreateController();
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
            _eventProcessorMock.Reset();
            _eventProcessorMock.Setup(s => s.ProcessEventAsync(eventId, eventName, It.IsAny<EventPayload>()))
                                    .Throws(someException);
            _loggerMock.Reset();

            var response = await controller.ReceiveEvent(request);

            _eventProcessorMock.Verify(s => s.ProcessEventAsync(eventId, eventName, It.IsAny<EventPayload>()), Times.Once());
            _loggerMock.Verify(logger => logger.Log(
                                            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                                            It.Is<EventId>(eventId => eventId.Id == 0),
                                            It.Is<It.IsAnyType>((@object, @type) => true),
                                            It.IsAny<Exception>(),
                                            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                                        Times.Once);
            _eventProcessorMock.Reset();
        }
    }
}
