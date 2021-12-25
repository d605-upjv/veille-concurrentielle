using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.EventOrchestrator.Lib.Controllers;
using VeilleConcurrentielle.Infrastructure.Core.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.ProductService.WebApp.Core.Services;

namespace VeilleConcurrentielle.ProductService.WebApp.Controllers
{
    public class ReceivedEventsController : ReceivedEventsControllerBase<ReceivedEventsController>
    {
        private readonly IEventProcessor _eventProcessor;
        public ReceivedEventsController(IReceivedEventRepository receivedEventRepository,
            ILogger<ReceivedEventsController> logger,
            IEventServiceClient eventServiceClient,
            IEventProcessor eventProcessor)
            : base(receivedEventRepository, logger, eventServiceClient)
        {
            _eventProcessor = eventProcessor;
        }

        public override ApplicationNames ApplicationName => ApplicationNames.ProductService;

        protected override async Task ProcessEvent(string eventId, EventNames eventName, EventPayload eventPayload)
        {
            try
            {
                await _eventProcessor.ProcessEventAsync(eventId, eventName, eventPayload);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process event {eventName} ({eventId})\nPayload: {SerializationUtils.Serialize(eventPayload)}");
            }
        }
    }
}
