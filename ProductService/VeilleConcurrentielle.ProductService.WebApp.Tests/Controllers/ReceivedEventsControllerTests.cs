using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Tests.Core.Controllers;
using VeilleConcurrentielle.ProductService.WebApp.Data;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Controllers
{
    public class ReceivedEventsControllerTests : ReceivedEventControllerTestsBase<ProductWebApp, Program, ProductDbContext>
    {
        [Fact]
        public override Task ReceiveEvent_Integration()
        {
            return base.ReceiveEvent_Integration();
        }

        [Fact]
        public override Task ReceiveEvent_Integration_InvalidSerializedPayload_ReturnsInternalServerError()
        {
            return base.ReceiveEvent_Integration_InvalidSerializedPayload_ReturnsInternalServerError();
        }
    }
}
