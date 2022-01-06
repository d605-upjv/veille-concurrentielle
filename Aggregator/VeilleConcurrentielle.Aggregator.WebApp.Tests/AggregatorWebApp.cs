extern alias mywebapp;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.TestLib;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests
{
    public class AggregatorWebApp : WebAppBase<mywebapp.Program, AggregatorDbContext>
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
                eventServiceClientMock.Setup(s => s.PushEventAsync(It.IsAny<PushEventClientRequest<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload>>()))
                                            .Returns((PushEventClientRequest<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload> request) => { 
                                                return Task.FromResult(new PushEventClientResponse<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload>() { Event = new AddOrUpdateProductRequestedEvent() { Id = $"{request.Name}EventUniqueId" } }); 
                                            });
                services.AddScoped((sp) => eventServiceClientMock.Object);
            });
            base.ConfigureWebHost(builder);
        }
    }
}
