using Microsoft.Extensions.Options;
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
        [Fact]
        public async Task PushEvent_Integration()
        {
            await using var application = new EventWebApp();
            using var httpClient = application.CreateClient();

            IOptions<ServiceUrlsOptions> serviceUrlOptions = Options.Create(new ServiceUrlsOptions()
            {
                EventUrl = httpClient.BaseAddress.ToString()
            });
            IEventServiceClient eventServiceClient = new EventServiceClient(httpClient, serviceUrlOptions);
            PushEventClientRequest<TestEvent, TestEventPayload> request = new PushEventClientRequest<TestEvent, TestEventPayload>();
            request.Name = EventNames.Test;
            request.Source = EventSources.Test;
            request.Payload = new TestEventPayload()
            {
                StringData = "String",
                IntData = 10
            };
            var response = await eventServiceClient.PushEvent(request);
            Assert.NotNull(response);
            Assert.NotNull(response.Event);
            Assert.Equal(request.Name, response.Event.Name);
            Assert.NotEmpty(response.Event.Id);
            Assert.Equal(request.Source, response.Event.Source);
            Assert.NotNull(response.Event.Payload);
            Assert.Equal(request.Payload.StringData, response.Event.Payload.StringData);
            Assert.Equal(request.Payload.IntData, response.Event.Payload.IntData);
        }
    }
}
