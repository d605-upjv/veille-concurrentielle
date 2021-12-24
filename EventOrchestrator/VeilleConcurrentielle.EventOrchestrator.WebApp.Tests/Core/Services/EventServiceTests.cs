using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Exceptions;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Core.Services;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Repositories;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using Xunit;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Tests.Core.Services
{
    public class EventServiceTests
    {
        public EventServiceTests()
        {
        }

        [Fact]
        public async Task PushEventAsync_AllServicesAreCalledCorrectly()
        {
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();

            var eventId = "EventId";
            var payload = new TestEventPayload()
            {
                StringData = "String",
                IntData = 10
            };
            var serializedPayload = SerializationUtils.Serialize(payload);
            eventRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<EventEntity>())).Returns((EventEntity entity) =>
            {
                entity.Id = eventId;
                return Task.CompletedTask;
            });
            eventRepositoryMock.Setup(s => s.GetByIdAsync(eventId))
                                    .Returns((string eventId) =>
                                     {
                                         return Task.FromResult<EventEntity?>(new EventEntity()
                                         {
                                             Id = eventId,
                                             Name = EventNames.Test.ToString(),
                                             Source = EventSources.Test.ToString(),
                                             SerializedPayload = serializedPayload
                                         });
                                     });
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventConsumerEntity>()));
            eventSubscriberRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventSubscriberEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventSubscriberEntity>()));
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);

            PushEventServerRequest request = new PushEventServerRequest()
            {
                EventName = EventNames.Test,
                Source = EventSources.Test,
                SerializedPayload = serializedPayload
            };
            var response = await eventService.PushEventAsync(request);
            Assert.NotNull(response);
            Assert.NotNull(response.Event);
            Assert.Equal(request.EventName, response.Event.Name);
            Assert.Equal(request.Source, response.Event.Source);
            Assert.Equal(request.SerializedPayload, response.Event.SerializedPayload);
            eventRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<EventEntity>()), Times.Once());
        }

        [Fact]
        public async Task GetNextEventAsync_AllServicesAreCalledCorrectly()
        {
            var existingEventId = "EventId";
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(s => s.GetNextEventId()).Returns(existingEventId);
            eventRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<EventEntity?>(new EventEntity()
                                        {
                                            Id = existingEventId,
                                            Name = EventNames.Test.ToString(),
                                            Source = EventSources.Test.ToString()
                                        }));
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()))
                .Returns(Task.FromResult(new List<EventConsumerEntity>
                {
                    new EventConsumerEntity() { ApplicationName= ApplicationNames.EventOrchestrator.ToString(), EventId = existingEventId}
                }));
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            eventSubscriberRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventSubscriberEntity, bool>>>()))
                .Returns(Task.FromResult(new List<EventSubscriberEntity>
                {
                    new EventSubscriberEntity(){ ApplicationName=ApplicationNames.EventOrchestrator.ToString(), EventName= EventNames.Test.ToString()},
                    new EventSubscriberEntity(){ ApplicationName=ApplicationNames.Aggregator.ToString(), EventName= EventNames.Test.ToString()},
                }));
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);
            var response = await eventService.GetNextEventAsync();
            Assert.NotNull(response);
            Assert.NotNull(response.Event);
            Assert.NotEmpty(response.Event.Subscribers);
            Assert.NotEmpty(response.Event.Consumers);
        }

        [Fact]
        public async Task GetNextEventAsync_WhenNoEvent_RetournNull()
        {
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(s => s.GetNextEventId()).Returns(() =>
            {
                return null;
            });
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()));
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            eventSubscriberRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventSubscriberEntity, bool>>>()));
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);
            var response = await eventService.GetNextEventAsync();
            Assert.Null(response);
        }

        [Fact]
        public async Task GetNextEventAsync_CheckIfAlreadyConsumed()
        {
            var existingEventId = "EventId";
            var isConsumed = false;
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(s => s.GetNextEventId()).Returns(() => isConsumed ? null : existingEventId);
            eventRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<EventEntity?>(new EventEntity()
                                        {
                                            Id = existingEventId,
                                            Name = EventNames.Test.ToString(),
                                            Source = EventSources.Test.ToString(),
                                            IsConsumed = isConsumed
                                        }));
            eventRepositoryMock.Setup(s => s.UpdateAsync(It.IsAny<EventEntity>()))
                                        .Callback(() =>
                                        {
                                            isConsumed = true;
                                        });
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()))
                .Returns(Task.FromResult(new List<EventConsumerEntity>
                {
                    new EventConsumerEntity() { ApplicationName= ApplicationNames.EventOrchestrator.ToString(), EventId = existingEventId},
                    new EventConsumerEntity() { ApplicationName= ApplicationNames.Aggregator.ToString(), EventId = existingEventId}
                }));
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            eventSubscriberRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventSubscriberEntity, bool>>>()))
                .Returns(Task.FromResult(new List<EventSubscriberEntity>
                {
                    new EventSubscriberEntity(){ ApplicationName=ApplicationNames.EventOrchestrator.ToString(), EventName= EventNames.Test.ToString()},
                    new EventSubscriberEntity(){ ApplicationName=ApplicationNames.Aggregator.ToString(), EventName= EventNames.Test.ToString()},
                }));
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);
            var response = await eventService.GetNextEventAsync();
            Assert.Null(response);
            eventRepositoryMock.Verify(s => s.UpdateAsync(It.IsAny<EventEntity>()), Times.Once());
        }

        [Fact]
        public async Task ConsumeEventAsync_AllServicesCalledCorrectly()
        {
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<EventEntity?>(new EventEntity()
                                        {
                                            Id = "EventId",
                                            Name = EventNames.Test.ToString(),
                                            Source = EventSources.Test.ToString()
                                        }))
                                        .Verifiable();
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventConsumerEntity>
                                        {
                                            new EventConsumerEntity()
                                            {
                                                Id = "Id",
                                                ApplicationName = ApplicationNames.Aggregator.ToString()
                                            }
                                        }))
                                        .Verifiable();
            eventSubscriberRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventSubscriberEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventSubscriberEntity>
                                        {
                                            new EventSubscriberEntity()
                                            {
                                                ApplicationName = ApplicationNames.Aggregator.ToString()
                                            },
                                            new EventSubscriberEntity()
                                            {
                                                ApplicationName = ApplicationNames.ProductService.ToString()
                                            }
                                        }))
                                        .Verifiable();
            eventConsumerRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<EventConsumerEntity>()))
                                        .Verifiable();
            eventRepositoryMock.Setup(s => s.UpdateAsync(It.IsAny<EventEntity>()))
                                        .Verifiable();
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);
            ConsumeEventServerRequest request = new ConsumeEventServerRequest()
            {
                EventId = "Eventid",
                ApplicationName = ApplicationNames.ProductService
            };

            var response = await eventService.ConsumeEventAsync(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Event);
            Assert.True(response.Event.IsConsumed);
            Assert.NotEmpty(response.Event.Subscribers);
            Assert.NotEmpty(response.Event.Consumers);
            eventRepositoryMock.VerifyAll();
            eventRepositoryMock.Verify(s => s.UpdateAsync(It.IsAny<EventEntity>()), Times.Once());
            eventConsumerRepositoryMock.VerifyAll();
            eventSubscriberRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task ConsumeEventAsync_WithExistingConsumer_ThrowsException()
        {
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<EventEntity?>(new EventEntity()
                                        {
                                            Id = "EventId",
                                            Name = EventNames.Test.ToString(),
                                            Source = EventSources.Test.ToString()
                                        }));
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventConsumerEntity>
                                        {
                                            new EventConsumerEntity()
                                            {
                                                Id = "Id",
                                                ApplicationName = ApplicationNames.Aggregator.ToString()
                                            }
                                        }));
            eventSubscriberRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventSubscriberEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventSubscriberEntity>
                                        {
                                            new EventSubscriberEntity()
                                            {
                                                ApplicationName = ApplicationNames.Aggregator.ToString()
                                            },
                                            new EventSubscriberEntity()
                                            {
                                                ApplicationName = ApplicationNames.ProductService.ToString()
                                            }
                                        }));
            eventConsumerRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<EventConsumerEntity>()));
            eventRepositoryMock.Setup(s => s.UpdateAsync(It.IsAny<EventEntity>()));
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);
            ConsumeEventServerRequest request = new ConsumeEventServerRequest()
            {
                EventId = "Eventid",
                ApplicationName = ApplicationNames.Aggregator
            };

            await Assert.ThrowsAsync<EventAlreadyConsumedException>(async () =>
            {
                await eventService.ConsumeEventAsync(request);
            });
        }

        [Fact]
        public async Task ConsumeEventAsync_WithAlreadyConsumedEvent_ThrowsException()
        {
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<EventEntity?>(new EventEntity()
                                        {
                                            Id = "EventId",
                                            Name = EventNames.Test.ToString(),
                                            Source = EventSources.Test.ToString(),
                                            IsConsumed = true
                                        }))
                                        .Verifiable();
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventConsumerEntity>()));
            eventSubscriberRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventSubscriberEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventSubscriberEntity>()));
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);
            ConsumeEventServerRequest request = new ConsumeEventServerRequest()
            {
                EventId = "Eventid",
                ApplicationName = ApplicationNames.Aggregator
            };

            await Assert.ThrowsAsync<EventAlreadyConsumedException>(async () =>
            {
                await eventService.ConsumeEventAsync(request);
            });
        }

        [Fact]
        public async Task ConsumeEventAsync_EventIsNotFullyConsumedIfSomeSusbcribersAreStillMissing()
        {
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<EventEntity?>(new EventEntity()
                                        {
                                            Id = "EventId",
                                            Name = EventNames.Test.ToString(),
                                            Source = EventSources.Test.ToString()
                                        }));
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventConsumerEntity>()));
            eventSubscriberRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventSubscriberEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventSubscriberEntity>
                                        {
                                            new EventSubscriberEntity()
                                            {
                                                ApplicationName = ApplicationNames.Aggregator.ToString()
                                            },
                                            new EventSubscriberEntity()
                                            {
                                                ApplicationName = ApplicationNames.ProductService.ToString()
                                            }
                                        }));
            eventConsumerRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<EventConsumerEntity>()));
            eventRepositoryMock.Setup(s => s.UpdateAsync(It.IsAny<EventEntity>()));
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);
            ConsumeEventServerRequest request = new ConsumeEventServerRequest()
            {
                EventId = "Eventid",
                ApplicationName = ApplicationNames.ProductService
            };

            var response = await eventService.ConsumeEventAsync(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Event);
            Assert.False(response.Event.IsConsumed);
            Assert.NotEmpty(response.Event.Subscribers);
            Assert.NotEmpty(response.Event.Consumers);
            eventRepositoryMock.Verify(s => s.UpdateAsync(It.IsAny<EventEntity>()), Times.Never());
        }

        [Fact]
        public async Task GetEvent_LoadAllDependencies()
        {
            var eventId = "EventId";
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<EventEntity?>(new EventEntity()
                                        {
                                            Id = eventId,
                                            Name = EventNames.Test.ToString(),
                                            Source = EventSources.Test.ToString()
                                        }))
                                        .Verifiable();
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventConsumerEntity>
                                        {
                                            new EventConsumerEntity()
                                            {
                                                Id = "Id",
                                                EventId= eventId,
                                                ApplicationName = ApplicationNames.Aggregator.ToString()
                                            }
                                        }))
                                        .Verifiable();
            eventSubscriberRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventSubscriberEntity, bool>>>()))
                                        .Returns(Task.FromResult(new List<EventSubscriberEntity>
                                        {
                                            new EventSubscriberEntity()
                                            {
                                                ApplicationName = ApplicationNames.Aggregator.ToString()
                                            },
                                            new EventSubscriberEntity()
                                            {
                                                ApplicationName = ApplicationNames.ProductService.ToString()
                                            }
                                        }))
                                        .Verifiable();
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);

            var response = await eventService.GetEventAsync(eventId);

            Assert.NotNull(response);
            Assert.NotEmpty(response.Subscribers);
            Assert.NotEmpty(response.Consumers);
            eventRepositoryMock.VerifyAll();
            eventConsumerRepositoryMock.VerifyAll();
            eventSubscriberRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetEvent_ReturnsNull_IfNotFound()
        {
            var eventId = "EventId";
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult<EventEntity?>(null))
                                        .Verifiable();
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);

            var response = await eventService.GetEventAsync(eventId);

            Assert.Null(response);
            eventRepositoryMock.VerifyAll();
        }
    }
}
