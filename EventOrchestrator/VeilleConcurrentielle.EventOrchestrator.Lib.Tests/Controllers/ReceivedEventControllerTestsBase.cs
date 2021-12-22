using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.Infrastructure.TestLib;
using Xunit;

namespace VeilleConcurrentielle.Infrastructure.Tests.Core.Controllers
{
    public abstract class ReceivedEventControllerTestsBase<TWebApp, TEntryPoint, TDbContext>
        where TWebApp : WebAppBase<TEntryPoint, TDbContext>, new()
        where TEntryPoint : class
        where TDbContext : DbContext
    {
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
                EventName = EventNames.NewPriceSubmitted,
                Source = EventSources.Test,
                DispatchedAt = DateTime.Now,
                SerializedPayload = "invalid serialized payload"
            };
            var response = await client.PostAsJsonAsync("/api/ReceivedEvents", request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
