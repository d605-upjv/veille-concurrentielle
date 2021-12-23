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
            eventRepositoryMock.Setup(s => s.InsertAsync(It.IsAny<EventEntity>())).Returns((EventEntity entity) =>
            {
                entity.Id = Guid.NewGuid().ToString();
                return Task.CompletedTask;
            });
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            IEventService eventService = new EventService(eventRepositoryMock.Object, eventSubscriberRepositoryMock.Object, eventConsumerRepositoryMock.Object);
            var payload = new TestEventPayload()
            {
                StringData = "String",
                IntData = 10
            };
            var serializedPayload = SerializationUtils.Serialize(payload);
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
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(s => s.GetNextEvent()).Returns(() =>
            {
                return new EventEntity()
                {
                    Id = "EventId",
                    Name = EventNames.Test.ToString(),
                    Source = EventSources.Test.ToString(),
                };
            });
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()))
                .Returns(Task.FromResult(new List<EventConsumerEntity>
                {
                    new EventConsumerEntity() { ApplicationName= ApplicationNames.EventOrchestrator.ToString(), EventId="EventId"},
                    new EventConsumerEntity() { ApplicationName= ApplicationNames.Aggregator.ToString(), EventId="Eventid"}
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
            Assert.NotEmpty(response.Subscribers);
            Assert.NotEmpty(response.Consumers);
        }

        [Fact]
        public async Task GetNextEventAsync_WhenNoEvent_RetournNull()
        {
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(s => s.GetNextEvent()).Returns(() =>
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
        public async Task ConsumeEventAsync_AllServicesCalledCorrectly()
        {
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult(new EventEntity()
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
            Assert.NotEmpty(response.Subscribers);
            Assert.NotEmpty(response.Consumers);
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
                                        .Returns(Task.FromResult(new EventEntity()
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
                                        .Returns(Task.FromResult(new EventEntity()
                                        {
                                            Id = "EventId",
                                            Name = EventNames.Test.ToString(),
                                            Source = EventSources.Test.ToString(),
                                            IsConsumed = true
                                        }))
                                        .Verifiable();
            eventConsumerRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventConsumerEntity, bool>>>()));
            eventSubscriberRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<EventSubscriberEntity, bool>>>()));
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
        public async Task ConsumeEventAsync_EventIsNotFullyConsumedIfSomeSusbcribersAreStillMissing()
        {
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            Mock<IEventSubscriberRepository> eventSubscriberRepositoryMock = new Mock<IEventSubscriberRepository>();
            Mock<IEventConsumerRepository> eventConsumerRepositoryMock = new Mock<IEventConsumerRepository>();
            eventRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                                        .Returns(Task.FromResult(new EventEntity()
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
            Assert.NotEmpty(response.Subscribers);
            Assert.NotEmpty(response.Consumers);
            eventRepositoryMock.Verify(s => s.UpdateAsync(It.IsAny<EventEntity>()), Times.Never());
        }
    }
}
