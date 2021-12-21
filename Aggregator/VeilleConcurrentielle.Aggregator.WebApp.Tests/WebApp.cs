using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using VeilleConcurrentielle.Aggregator.WebApp.Data;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models.Events;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.TestLib;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests
{
    internal class WebApp : WebAppBase<Program, AggregatorDbContext>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var existingEventServiceClient = services.SingleOrDefault(s => s.ServiceType == typeof(IEventServiceClient));
                if (existingEventServiceClient != null)
                {
                    services.Remove(existingEventServiceClient);
                }
                var eventServiceClientMock = new Mock<IEventServiceClient>();
                eventServiceClientMock.Setup(s => s.PushEvent(It.IsAny<PushEventClientRequest<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload>>()))
                                            .Returns((PushEventClientRequest<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload> request) => { 
                                                return Task.FromResult(new PushEventClientResponse<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload>() { Event = new AddOrUpdateProductRequestedEvent() { Id = $"{request.Name}EventUniqueId" } }); 
                                            });
                services.AddScoped((sp) => eventServiceClientMock.Object);
            });
            base.ConfigureWebHost(builder);
        }
    }
}
