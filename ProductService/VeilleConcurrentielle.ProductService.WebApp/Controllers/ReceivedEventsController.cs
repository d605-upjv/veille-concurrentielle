using VeilleConcurrentielle.Infrastructure.Core.Controllers;
using VeilleConcurrentielle.Infrastructure.Core.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Controllers
{
    public class ReceivedEventsController : ReceivedEventsControllerBase<ReceivedEventsController>
    {
        public ReceivedEventsController(IReceivedEventRepository receivedEventRepository, ILogger<ReceivedEventsController> logger) : base(receivedEventRepository, logger)
        {
        }

        protected override async Task ProcessEvent(EventNames eventName, EventPayload eventPayload)
        {
        }
    }
}
