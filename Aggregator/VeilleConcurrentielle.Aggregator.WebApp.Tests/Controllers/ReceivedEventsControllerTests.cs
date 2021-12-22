using System.Threading.Tasks;
using VeilleConcurrentielle.Aggregator.WebApp.Data;
using VeilleConcurrentielle.Infrastructure.Tests.Core.Controllers;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers
{
    public class ReceivedEventsControllerTests : ReceivedEventControllerTestsBase<AggregatorWebApp, Program, AggregatorDbContext>
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
