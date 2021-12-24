using Microsoft.AspNetCore.Mvc;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Web;

namespace VeilleConcurrentielle.Aggregator.WebApp.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        private readonly IEventServiceClient _eventServiceClient;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IEventServiceClient eventServiceClient, ILogger<ProductsController> logger)
        {
            _eventServiceClient = eventServiceClient;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostAddOrEditProduct([FromBody] AddOrEditProductModels.AddOrEditProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                PushEventClientRequest<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload> eventRequest = new PushEventClientRequest<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload>();
                eventRequest.Name = EventNames.AddOrUpdateProductRequested;
                eventRequest.Source = EventSources.ProductService;
                eventRequest.Payload = request;
                var response = await _eventServiceClient.PushEventAsync(eventRequest);
                return Ok(new AddOrEditProductModels.AddOrEditProductResponse(response.Event.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to post AddOrEditProduct event");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
