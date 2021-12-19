using Microsoft.AspNetCore.Mvc;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Core.Services;
using VeilleConcurrentielle.Infrastructure.Web;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Controllers
{
    public class EventsController: ApiControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventsController> _logger;
        public EventsController(IEventService eventRepository, ILogger<EventsController> logger)
        {
            _eventService = eventRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PushEventServerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _eventService.PushEventAsync(request);
                if (response == null)
                {
                    return NotFound();
                }
                return Created($"/api/Events/{response.Event.Id}", response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to push event");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
