using AutoMapper;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Repositories;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Core.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<PushEventServerResponse> PushEventAsync(PushEventServerRequest request)
        {
            EventEntity entity = new EventEntity();
            entity.Name = request.EventName.ToString();
            entity.SerializedPayload = request.SerializedPayload;
            entity.Source = request.Source.ToString();
            entity.CreatedAt = DateTime.Now;
            await _eventRepository.InsertAsync(entity);
            Event evt = _mapper.Map<Event>(entity);
            return new PushEventServerResponse() { Event = evt };
        }
    }
}
