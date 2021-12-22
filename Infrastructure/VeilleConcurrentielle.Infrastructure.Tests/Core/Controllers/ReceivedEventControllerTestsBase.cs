using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
            var payload = new NewPriceSubmittedEventPayload()
            {
                ProductId = "Product1",
                Price = 100,
                Source = PriceSources.PriceSubmissionApi
            };
            var serializedPayload = SerializationUtils.Serialize(payload);
            ReceivedEventModels.ReceivedEentRequest request = new ReceivedEventModels.ReceivedEentRequest()
            {
                EventName = EventNames.NewPriceSubmitted,
                Source = EventSources.Test,
                SubmittedAt = DateTime.Now,
                SerializedPayload = serializedPayload
            };
            var response = await client.PostAsJsonAsync("/api/ReceivedEvents", request);
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<ReceivedEventModels.ReceivedEentResponse>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent.Id);
        }

        public virtual async Task ReceiveEvent_Integration_InvalidSerializedPayload_ReturnsInternalServerError()
        {
            await using var application = new TWebApp();
            using var client = application.CreateClient();
            ReceivedEventModels.ReceivedEentRequest request = new ReceivedEventModels.ReceivedEentRequest()
            {
                EventName = EventNames.NewPriceSubmitted,
                Source = EventSources.Test,
                SubmittedAt = DateTime.Now,
                SerializedPayload = "invalid serialized payload"
            };
            var response = await client.PostAsJsonAsync("/api/ReceivedEvents", request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
