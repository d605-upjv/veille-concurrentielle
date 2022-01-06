extern alias mywebapp;

using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Controllers;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Tests.Core.Controllers;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Controllers
{
    public class ReceivedEventsControllerTests : ReceivedEventControllerTestsBase<ProductWebApp, mywebapp.Program, ProductDbContext, ReceivedEventsController>
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
