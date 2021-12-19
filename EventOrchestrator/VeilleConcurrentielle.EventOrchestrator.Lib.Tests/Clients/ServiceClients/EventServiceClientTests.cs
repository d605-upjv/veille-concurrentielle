using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models.Events;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using Xunit;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Tests.Clients.ServiceClients
{
    public class EventServiceClientTests
    {
        [Fact]
        public async Task PushEvent_NewPriceSubmitted()
        {
            await using var application = new WebApp();
            using var httpClient = application.CreateClient();

            IEventServiceClient eventServiceClient = new EventServiceClient(httpClient);
            PushEventClientRequest<NewPriceSubmittedEvent, NewPriceSubmittedEventPayload> request = new PushEventClientRequest<NewPriceSubmittedEvent, NewPriceSubmittedEventPayload>();
            request.Name = EventNames.NewPriceSubmitted;
            request.Source = EventSources.Test;
            request.Payload = new NewPriceSubmittedEventPayload()
            {
                ProductId = "Product1",
                Price = 100,
                Source = PriceSources.PriceSubmissionApi
            };
            var response = await eventServiceClient.PushEvent(request);
            Assert.NotNull(response);
            Assert.NotNull(response.Event);
            Assert.Equal(request.Name, response.Event.Name);
            Assert.NotEmpty(response.Event.Id);
            Assert.Equal(request.Source, response.Event.Source);
            Assert.NotNull(response.Event.Payload);
            Assert.Equal(request.Payload.ProductId, response.Event.Payload.ProductId);
            Assert.Equal(request.Payload.Price, response.Event.Payload.Price);
            Assert.Equal(request.Payload.Source, response.Event.Payload.Source);
        }
    }
}
