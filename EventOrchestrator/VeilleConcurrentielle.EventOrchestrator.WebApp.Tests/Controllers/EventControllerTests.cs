using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.Infrastructure.Framework;
using Xunit;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Tests.Controllers
{
    public class EventControllerTests
    {
        [Fact]
        public async Task PushEventAsync_WithCorrectValues()
        {
            await using var application = new WebApp();

            var request = new PushEventServerRequest()
            {
                EventName = "SomeEvent",
                Source = "UnitTest",
                SerializedPayload = "nothing"
            };
            using var client = application.CreateClient();
            var response = await client.PostAsJsonAsync("/api/Events", request);
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<PushEventServerResponse>(response);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(responseContent.Event);
            Assert.NotNull(responseContent.Event.Id);
        }

        [Fact]
        public async Task PushEventAsync_WithIncorrectAttributes_ReturnsBadRequest()
        {
            await using var application = new WebApp();

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
    }
}
