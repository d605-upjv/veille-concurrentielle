using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Controllers;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data;
using VeilleConcurrentielle.Infrastructure.Tests.Core.Controllers;
using Xunit;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Tests.Controllers
{
    public class ReceivedEventsControllerTests : ReceivedEventControllerTestsBase<EventWebApp, Program, EventDbContext, ReceivedEventsController>
    {
        protected override ReceivedEventsController CreateController()
        {
            return new ReceivedEventsController(_receivedEventRepositorMock.Object, _loggerMock.Object, _eventServiceClientMock.Object, _eventProcessorMock.Object);
        }

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

        [Fact]
        public override Task ReceiveEvent_ProcessEvent_IsTriggered()
        {
            return base.ReceiveEvent_ProcessEvent_IsTriggered();
        }

        [Fact]
        public override Task ReceiveEvent_ProcessEvent_WhenFailed_CatchAndLog()
        {
            return base.ReceiveEvent_ProcessEvent_WhenFailed_CatchAndLog();
        }
    }
}
