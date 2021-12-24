using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.EventOrchestrator.Lib.Controllers;
using VeilleConcurrentielle.Infrastructure.Core.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Controllers
{
    public class ReceivedEventsController : ReceivedEventsControllerBase<ReceivedEventsController>
    {
        public ReceivedEventsController(IReceivedEventRepository receivedEventRepository, ILogger<ReceivedEventsController> logger, IEventServiceClient eventServiceClient) 
            : base(receivedEventRepository, logger, eventServiceClient)
        {
        }

        public override ApplicationNames ApplicationName => ApplicationNames.ProductService;

        protected override async Task ProcessEvent(string eventId, EventNames eventName, EventPayload eventPayload)
        {
        }
    }
}
