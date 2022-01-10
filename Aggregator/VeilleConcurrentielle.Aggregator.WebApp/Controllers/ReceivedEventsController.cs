using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.EventOrchestrator.Lib.Controllers;
using VeilleConcurrentielle.Infrastructure.Core.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Services;

namespace VeilleConcurrentielle.Aggregator.WebApp.Controllers
{
    public class ReceivedEventsController : ReceivedEventsControllerBase<ReceivedEventsController>
    {
        public ReceivedEventsController(IReceivedEventRepository receivedEventRepository, ILogger<ReceivedEventsController> logger, IEventServiceClient eventServiceClient, IEventProcessor eventProcessor)
            : base(receivedEventRepository, logger, eventServiceClient, eventProcessor)
        {
        }

        public override ApplicationNames ApplicationName => ApplicationNames.Aggregator;
    }
}
