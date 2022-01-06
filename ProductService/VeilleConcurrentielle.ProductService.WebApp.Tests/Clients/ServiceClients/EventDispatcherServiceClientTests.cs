extern alias mywebapp;

using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Tests.Clients.ServiceClients;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Clients.ServiceClients
{
    public class EventDispatcherServiceClientTests : EventDispatchServiceClientTestsBase<ProductWebApp, mywebapp.Program, ProductDbContext>
    {
        [Fact]
        public override Task DispatchEvent_Integration()
        {
            return base.DispatchEvent_Integration();
        }
    }
}
