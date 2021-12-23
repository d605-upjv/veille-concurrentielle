using VeilleConcurrentielle.EventOrchestrator.Lib.Exceptions;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Framework;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Core.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventSubscriberRepository _eventSubscriberRepository;
        private readonly IEventConsumerRepository _eventConsumerRepository;
        public EventService(IEventRepository eventRepository, IEventSubscriberRepository eventSubscriberRepository, IEventConsumerRepository eventConsumerRepository)
        {
            _eventRepository = eventRepository;
            _eventSubscriberRepository = eventSubscriberRepository;
            _eventConsumerRepository = eventConsumerRepository;
        }

        public async Task<PushEventServerResponse> PushEventAsync(PushEventServerRequest request)
        {
            EventEntity entity = new EventEntity();
            entity.Name = request.EventName.ToString();
            entity.SerializedPayload = request.SerializedPayload;
            entity.Source = request.Source.ToString();
            entity.CreatedAt = DateTime.Now;
            entity.IsConsumed = false;
            await _eventRepository.InsertAsync(entity);
            Event event_ = await GetEventAsync(entity.Id);
            return new PushEventServerResponse() { Event = event_ };
        }

        public async Task<GetNextEventServerResponse> GetNextEventAsync()
        {
            var nextEventId = _eventRepository.GetNextEventId();
            if (nextEventId != null)
            {
                Event event_ = await GetEventAsync(nextEventId);
                return new GetNextEventServerResponse()
                {
                    Event = event_
                };
            }
            return null;
        }

        public async Task<ConsumeEventServerResponse> ConsumeEventAsync(ConsumeEventServerRequest request)
        {
            var event_ = await GetEventAsync(request.EventId);
            if (event_ != null)
            {
                if (event_.IsConsumed)
                {
                    throw new EventAlreadyConsumedException();
                }
                if (event_.Consumers.Exists(c => c.ApplicationName == request.ApplicationName))
                {
                    throw new EventAlreadyConsumedException();
                }
                var consumer = new EventConsumerEntity()
                {
                    ApplicationName = request.ApplicationName.ToString(),
                    EventId = request.EventId,
                    CreatedAt = DateTime.Now
                };
                await _eventConsumerRepository.InsertAsync(consumer);
                event_.Consumers.Add(new EventConsumer()
                {
                    Id = consumer.Id,
                    ApplicationName = EnumUtils.GetValueFromString<ApplicationNames>(consumer.ApplicationName),
                    CreatedAt = consumer.CreatedAt
                });
                if (event_.Subscribers.All(s => event_.Consumers.Exists(c => c.ApplicationName == s.ApplicationName)))
                {
                    event_.IsConsumed = true;
                    await _eventRepository.UpdateAsync(new EventEntity()
                    {
                        Id = event_.Id,
                        Name = event_.Name.ToString(),
                        Source = event_.Source.ToString(),
                        SerializedPayload = event_.SerializedPayload,
                        CreatedAt = event_.CreatedAt,
                        IsConsumed = event_.IsConsumed
                    });
                }
                return new ConsumeEventServerResponse()
                {
                    Event = event_
                };
            }
            return null;
        }

        public async Task<Event> GetEventAsync(string eventId)
        {
            var entity = await this._eventRepository.GetByIdAsync(eventId);
            if (entity != null)
            {
                var subscribers = (await _eventSubscriberRepository.GetAllAsync(e => e.EventName == entity.Name))
                                    .Select(e => new EventSubscriber()
                                    {
                                        ApplicationName = EnumUtils.GetValueFromString<ApplicationNames>(e.ApplicationName)
                                    }).ToList();
                var consumers = (await _eventConsumerRepository.GetAllAsync(e => e.EventId == entity.Id))
                                    .Select(e => new EventConsumer()
                                    {
                                        Id = e.Id,
                                        ApplicationName = EnumUtils.GetValueFromString<ApplicationNames>(e.ApplicationName),
                                        CreatedAt = e.CreatedAt
                                    }).ToList();
                return new Event()
                {
                    Id = entity.Id,
                    Name = EnumUtils.GetValueFromString<EventNames>(entity.Name),
                    Source = EnumUtils.GetValueFromString<EventSources>(entity.Source),
                    CreatedAt = entity.CreatedAt,
                    SerializedPayload = entity.SerializedPayload,
                    IsConsumed = entity.IsConsumed,
                    Consumers = consumers,
                    Subscribers = subscribers
                };
            }
            return null;
        }
    }
}
