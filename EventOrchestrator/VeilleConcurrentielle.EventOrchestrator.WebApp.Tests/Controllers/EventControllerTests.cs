using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using Xunit;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Tests.Controllers
{
    public class EventControllerTests
    {
        [Fact]
        public async Task PushEventAsync_WithCorrectValues()
        {
            await using var application = new EventWebApp();
            using var client = application.CreateClient();

            var payload = new NewPriceSubmittedEventPayload()
            {
                ProductId = "Product1",
                Price = 10,
                Source = PriceSources.PriceSubmissionApi
            };
            var serializedPayload = SerializationUtils.Serialize(payload);
            var request = new PushEventServerRequest()
            {
                EventName = EventNames.NewPriceSubmitted,
                Source = EventSources.Test,
                SerializedPayload = serializedPayload
            };
            
            var response = await client.PostAsJsonAsync("/api/Events", request);
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<PushEventServerResponse>(response);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(responseContent.Event);
            Assert.NotNull(responseContent.Event.Id);
            Assert.NotNull(responseContent.Event.Subscribers);
            Assert.NotNull(responseContent.Event.Consumers);
            Assert.False(responseContent.Event.IsConsumed);
            Assert.Equal(request.Source, responseContent.Event.Source);
        }

        [Fact]
        public async Task PushEventAsync_WithIncorrectAttributes_ReturnsBadRequest()
        {
            await using var application = new EventWebApp();

            dynamic request = new
            {
                EventNameX = "Unknown",
                Source = "UnitTest",
                SerializedPayload = "nothing"
            };
            using var client = application.CreateClient();
            var payload = HttpClientUtils.CreateHttpContent(request);
            var response = await client.PostAsync("/api/Events", payload);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetNextEvent_Integration()
        {
            await using var application = new EventWebApp();
            using var client = application.CreateClient();

            var response = await client.GetAsync("/api/Events/next");

            Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task ConsumeEvent_Integration()
        {
            await using var application = new EventWebApp();
            using var client = application.CreateClient();

            ConsumeEventServerRequest request = new ConsumeEventServerRequest()
            {
                EventId="EventId",
                ApplicationName= ApplicationNames.Aggregator
            };
            var payload = HttpClientUtils.CreateHttpContent(request);
            var response = await client.PostAsync("/api/Events/consume", payload);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetEventt_Integration()
        {
            await using var application = new EventWebApp();
            using var client = application.CreateClient();

            var eventId = "UnknownEventId";
            var response = await client.GetAsync($"/api/Events/{eventId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
