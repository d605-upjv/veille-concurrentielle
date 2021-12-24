using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Configurations;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.Infrastructure.TestLib;
using Xunit;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Tests.Clients.ServiceClients
{
    public abstract class EventDispatchServiceClientTestsBase<TWebApp, TEntryPoint, TDbContext>
        where TWebApp : WebAppBase<TEntryPoint, TDbContext>, new()
        where TEntryPoint : class
        where TDbContext : DbContext
    {
        protected readonly Mock<ILogger<EventDispatcherServiceClient>> _loggerMock;
        public EventDispatchServiceClientTestsBase()
        {
            _loggerMock = new Mock<ILogger<EventDispatcherServiceClient>>();
        }
        public virtual async Task DispatchEvent_Integration()
        {
            await using var application = new TWebApp();
            using var httpClient = application.CreateClient();

            IOptions<ServiceUrlsOptions> serviceUrlOptions = Options.Create(new ServiceUrlsOptions()
            {
                EventUrl = httpClient.BaseAddress.ToString(),
                AggregatorUrl = httpClient.BaseAddress.ToString(),
                ProductUrl = httpClient.BaseAddress.ToString(),
            });
            IEventDispatcherServiceClient eventDispatcherServiceClient = new EventDispatcherServiceClient(httpClient, serviceUrlOptions, _loggerMock.Object);
            var payload = new TestEventPayload()
            {
                StringData = "String",
                IntData = 50
            };
            var serializedPayload = SerializationUtils.Serialize(payload);
            DispatchEventClientRequest clientRequest = new DispatchEventClientRequest()
            {
                EventId = "EventId",
                ApplicationName = ApplicationNames.EventOrchestrator,
                EventName = EventNames.Test,
                Source = EventSources.Test,
                CreatedAt = DateTime.Now.AddMinutes(-1),
                DispatchedAt = DateTime.Now,
                SerializedPayload = serializedPayload
            };
            var clientResponse = await eventDispatcherServiceClient.DispatchEventAsync(clientRequest);
            Assert.NotNull(clientResponse);
            Assert.NotNull(clientResponse.ReceivedEventId);
        }
    }
}
