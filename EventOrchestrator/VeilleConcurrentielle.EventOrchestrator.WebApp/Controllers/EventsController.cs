using Microsoft.AspNetCore.Mvc;
using VeilleConcurrentielle.EventOrchestrator.Lib.Exceptions;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Core.Services;
using VeilleConcurrentielle.Infrastructure.Web;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Controllers
{
    public class EventsController : ApiControllerBase
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
                return Created($"/api/Events/{response.Event.Id}", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to push event");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("next")]
        public async Task<IActionResult> GetNextEvent()
        {
            try
            {
                var response = await _eventService.GetNextEventAsync();
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get next event");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("consume")]
        public async Task<IActionResult> ConsumeEvent([FromBody] ConsumeEventServerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _eventService.ConsumeEventAsync(request);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (EventAlreadyConsumedException)
            {
                return BadRequest("Event is already consumed");
            }
            catch(ApplicationSubscriptionNotFoundException)
            {
                return BadRequest("Application subscription not found");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to get next event");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
