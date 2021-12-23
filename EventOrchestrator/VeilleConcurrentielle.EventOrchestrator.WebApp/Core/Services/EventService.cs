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
            Event evt = EventFromEntity(entity);
            return new PushEventServerResponse() { Event = evt };
        }

        public async Task<GetNextEventServerResponse> GetNextEventAsync()
        {
            var nextEventEntity = _eventRepository.GetNextEvent();
            if (nextEventEntity != null)
            {
                Event evt = EventFromEntity(nextEventEntity);
                var subscribers = (await _eventSubscriberRepository.GetAllAsync(e => e.EventName == nextEventEntity.Name))
                                    .Select(e => new EventSubscriber()
                                    {
                                        ApplicationName = EnumUtils.GetValueFromString<ApplicationNames>(e.ApplicationName)
                                    }).ToList();
                var consumers = (await _eventConsumerRepository.GetAllAsync(e => e.EventId == nextEventEntity.Id))
                                    .Select(e => new EventConsumer()
                                    {
                                        Id = e.Id,
                                        ApplicationName = EnumUtils.GetValueFromString<ApplicationNames>(e.ApplicationName),
                                        CreatedAt = e.CreatedAt
                                    }).ToList();
                return new GetNextEventServerResponse()
                {
                    Event = evt,
                    Subscribers = subscribers,
                    Consumers = consumers
                };
            }
            return null;
        }

        public async Task<ConsumeEventServerResponse> ConsumeEventAsync(ConsumeEventServerRequest request)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
            if (eventEntity != null)
            {
                if (eventEntity.IsConsumed)
                {
                    throw new EventAlreadyConsumedException();
                }
                var subscribers = (await _eventSubscriberRepository.GetAllAsync(e => e.EventName == eventEntity.Name))
                                        .Select(s => new EventSubscriber()
                                        {
                                            ApplicationName = EnumUtils.GetValueFromString<ApplicationNames>(s.ApplicationName)
                                        }).ToList();
                if (!subscribers.Exists(s => s.ApplicationName == request.ApplicationName))
                {
                    throw new ApplicationSubscriptionNotFoundException();
                }
                var consumers = (await _eventConsumerRepository.GetAllAsync(e => e.EventId == request.EventId))
                                        .Select(c => new EventConsumer()
                                        {
                                            Id = c.Id,
                                            ApplicationName = EnumUtils.GetValueFromString<ApplicationNames>(c.ApplicationName),
                                            CreatedAt = c.CreatedAt
                                        }).ToList();
                if (consumers.Exists(c => c.ApplicationName == request.ApplicationName))
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
                consumers.Add(new EventConsumer()
                {
                    Id = consumer.Id,
                    ApplicationName = EnumUtils.GetValueFromString<ApplicationNames>(consumer.ApplicationName),
                    CreatedAt = consumer.CreatedAt
                });
                if (subscribers.All(s => consumers.Exists(c => c.ApplicationName == s.ApplicationName)))
                {
                    eventEntity.IsConsumed = true;
                    await _eventRepository.UpdateAsync(eventEntity);
                }
                return new ConsumeEventServerResponse()
                {
                    Event = EventFromEntity(eventEntity),
                    Consumers = consumers,
                    Subscribers = subscribers
                };
            }
            return null;
        }

        public static Event EventFromEntity(EventEntity eventEntity)
        {
            return new Event()
            {
                Id = eventEntity.Id,
                Name = EnumUtils.GetValueFromString<EventNames>(eventEntity.Name),
                Source = EnumUtils.GetValueFromString<EventSources>(eventEntity.Source),
                CreatedAt = eventEntity.CreatedAt,
                SerializedPayload = eventEntity.SerializedPayload,
                IsConsumed = eventEntity.IsConsumed
            };
        }
    }
}
