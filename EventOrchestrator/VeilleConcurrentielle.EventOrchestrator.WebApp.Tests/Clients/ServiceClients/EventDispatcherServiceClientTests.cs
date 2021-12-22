using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Tests.Clients.ServiceClients;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data;
using Xunit;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Tests.Clients.ServiceClients
{
    public class EventDispatcherServiceClientTests : EventDispatchServiceClientTestsBase<EventWebApp, Program, EventDbContext>
    {
        [Fact]
        public override Task DispatchEvent_Integration()
        {
            return base.DispatchEvent_Integration();
        }
    }
}
