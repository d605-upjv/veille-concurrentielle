using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VeilleConcurrentielle.Infrastructure.Core.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Core.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.Core.Framework;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.Infrastructure.Web;

namespace VeilleConcurrentielle.Infrastructure.Core.Controllers
{
    public abstract class ReceivedEventsControllerBase<TController> : ApiControllerBase where TController : ApiControllerBase
    {
        protected readonly ILogger<TController> _logger;
        protected readonly IReceivedEventRepository _receivedEentRepository;

        public ReceivedEventsControllerBase(IReceivedEventRepository receivedEventRepository, ILogger<TController> logger)
        {
            _receivedEentRepository = receivedEventRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveEvent([FromBody] ReceivedEventModels.ReceivedEentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                _logger.LogInformation($"Event received: {SerializationUtils.Serialize(request)}");
                var storedId = await StoreReceivedEvent(request);
                var payloadType = EventResolver.GetEventPayloadType(request.EventName);
                var payload = SerializationUtils.Deserialize(request.SerializedPayload, payloadType) as EventPayload;
                await ProcessEvent(request.EventName, payload);
                return Ok(new ReceivedEventModels.ReceivedEentResponse() { Id = storedId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to receive event");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        protected virtual async Task<string> StoreReceivedEvent(ReceivedEventModels.ReceivedEentRequest request)
        {
            ReceivedEventEntity receivedEventEntity = new ReceivedEventEntity();
            receivedEventEntity.Name = request.EventName.ToString();
            receivedEventEntity.SubmittedAt = request.SubmittedAt;
            receivedEventEntity.Source = request.Source.ToString();
            receivedEventEntity.CreatedAt = DateTime.Now;
            receivedEventEntity.SerializedPayload = request.SerializedPayload;
            await _receivedEentRepository.InsertAsync(receivedEventEntity);
            return receivedEventEntity.Id;
        }

        protected abstract Task ProcessEvent(EventNames eventName, EventPayload eventPayload);
    }
}
