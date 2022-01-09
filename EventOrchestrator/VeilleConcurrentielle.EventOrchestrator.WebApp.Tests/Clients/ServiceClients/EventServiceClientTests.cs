using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Configurations;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using Xunit;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Tests.Clients.ServiceClients
{
    public class EventServiceClientTests
    {
        private readonly Mock<ILogger<EventServiceClient>> _loggerMock;
        public EventServiceClientTests()
        {
            _loggerMock = new Mock<ILogger<EventServiceClient>>();
        }
        [Fact]
        public async Task PushEvent_Integration()
        {
            await using var application = new EventWebApp();
            using var httpClient = application.CreateClient();

            IOptions<ServiceUrlsOptions> serviceUrlOptions = Options.Create(new ServiceUrlsOptions()
            {
                EventUrl = httpClient.BaseAddress.ToString()
            });
            IEventServiceClient eventServiceClient = new EventServiceClient(httpClient, serviceUrlOptions, _loggerMock.Object);
            PushEventClientRequest<TestEvent, TestEventPayload> request = new PushEventClientRequest<TestEvent, TestEventPayload>();
            request.Name = EventNames.Test;
            request.Source = EventSources.Test;
            request.Payload = new TestEventPayload()
            {
                StringData = "String",
                IntData = 10
            };
            var response = await eventServiceClient.PushEventAsync(request);
            Assert.NotNull(response);
            Assert.NotNull(response.Event);
            Assert.Equal(request.Name, response.Event.Name);
            Assert.NotEmpty(response.Event.Id);
            Assert.Equal(request.Source, response.Event.Source);
            Assert.NotNull(response.Event.Payload);
            Assert.Equal(request.Payload.StringData, response.Event.Payload.StringData);
            Assert.Equal(request.Payload.IntData, response.Event.Payload.IntData);
        }

        [Fact]
        public async Task GetNextEvent_Integration()
        {
            await using var application = new EventWebApp();
            using var httpClient = application.CreateClient();

            IOptions<ServiceUrlsOptions> serviceUrlOptions = Options.Create(new ServiceUrlsOptions()
            {
                EventUrl = httpClient.BaseAddress.ToString()
            });
            IEventServiceClient eventServiceClient = new EventServiceClient(httpClient, serviceUrlOptions, _loggerMock.Object);

            var response = await eventServiceClient.GetNextEventAsync();

            if (response != null)
            {
                Assert.NotNull(response.Event);
                Assert.NotNull(response.Event.Id);
                Assert.False(response.Event.IsConsumed);
            }
            else
            {
                Assert.Null(response);
            }
        }

        [Fact]
        public async Task ConsumeEvent_Integration()
        {
            await using var application = new EventWebApp();
            using var httpClient = application.CreateClient();

            IOptions<ServiceUrlsOptions> serviceUrlOptions = Options.Create(new ServiceUrlsOptions()
            {
                EventUrl = httpClient.BaseAddress.ToString()
            });
            IEventServiceClient eventServiceClient = new EventServiceClient(httpClient, serviceUrlOptions, _loggerMock.Object);
            ConsumeEventClientRequest request = new ConsumeEventClientRequest();
            request.EventId = "UnknownEvent";
            var response = await eventServiceClient.ConsumeEventAsync(request);

            Assert.Null(response);
        }
    }
}
