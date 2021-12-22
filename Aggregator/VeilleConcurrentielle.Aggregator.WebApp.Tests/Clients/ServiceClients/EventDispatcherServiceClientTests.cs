extern alias mywebapp;

using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Data;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Tests.Clients.ServiceClients;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Clients.ServiceClients
{
    public class EventDispatcherServiceClientTests : EventDispatchServiceClientTestsBase<AggregatorWebApp, mywebapp.Program, AggregatorDbContext>
    {
        [Fact]
        public override Task DispatchEvent_Integration()
        {
            return base.DispatchEvent_Integration();
        }
    }
}
