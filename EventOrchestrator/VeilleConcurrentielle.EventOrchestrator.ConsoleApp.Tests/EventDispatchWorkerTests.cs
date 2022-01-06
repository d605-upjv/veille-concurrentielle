using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using Xunit;

namespace VeilleConcurrentielle.EventOrchestrator.ConsoleApp.Tests
{
    public class EventDispatchWorkerTests
    {
        private readonly Mock<ILogger<EventDispatchWorker>> _loggerMock;
        private readonly Mock<IAppTerminator> _appTerminatorMock;
        public EventDispatchWorkerTests()
        {
            _loggerMock = new Mock<ILogger<EventDispatchWorker>>();
            _appTerminatorMock = new Mock<IAppTerminator>();
        }

        [Fact]
        public async Task Run_WithSubscribers()
        {
            Mock<IEventServiceClient> eventServiceClientMock = new Mock<IEventServiceClient>();
            Mock<IEventDispatcherServiceClient> eventDispatcherServiceClientMock = new Mock<IEventDispatcherServiceClient>();
            IOptions<WorkerConfigOptions> workerConfigOptions = Options.Create<WorkerConfigOptions>(new WorkerConfigOptions()
            {
                InfiniteRun = false,
            });
            Event event_ = new Event()
            {
                Id = "EventId",
                Name = EventNames.Test,
                Source = EventSources.Test,
                Consumers = new List<EventConsumer>(),
                Subscribers = new List<EventSubscriber>
                {
                    new EventSubscriber(){ ApplicationName= ApplicationNames.ProductService },
                    new EventSubscriber(){ ApplicationName= ApplicationNames.Aggregator }
                }
            };
            eventServiceClientMock.Setup(s => s.GetNextEventAsync())
                                    .Returns(Task.FromResult<GetNextEventClientResponse?>(new GetNextEventClientResponse()
                                    {
                                        Event = event_
                                    }));
            eventServiceClientMock.Setup(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()));
            eventDispatcherServiceClientMock.Setup(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()))
                                    .Returns((DispatchEventClientRequest request) =>
                                    {
                                        return Task.FromResult<DispatchEventClientResponse?>(new DispatchEventClientResponse()
                                        {
                                            ReceivedEventId = "ReceivedEevntId"
                                        });
                                    });
            IEventDispatchWorker eventDispatchWorker = new EventDispatchWorker(_loggerMock.Object, eventServiceClientMock.Object, eventDispatcherServiceClientMock.Object, workerConfigOptions, _appTerminatorMock.Object);

            await eventDispatchWorker.Run();

            eventServiceClientMock.Verify(s => s.GetNextEventAsync(), Times.Once());
            eventDispatcherServiceClientMock.Verify(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()), Times.Exactly(2));
            eventServiceClientMock.Verify(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Run_WithoutSubscribers()
        {
            Mock<IEventServiceClient> eventServiceClientMock = new Mock<IEventServiceClient>();
            Mock<IEventDispatcherServiceClient> eventDispatcherServiceClientMock = new Mock<IEventDispatcherServiceClient>();
            IOptions<WorkerConfigOptions> workerConfigOptions = Options.Create<WorkerConfigOptions>(new WorkerConfigOptions()
            {
                InfiniteRun = false
            });
            Event event_ = new Event()
            {
                Id = "EventId",
                Name = EventNames.Test,
                Source = EventSources.Test,
                Consumers = new List<EventConsumer>(),
                Subscribers = new List<EventSubscriber>()
            };
            eventServiceClientMock.Setup(s => s.GetNextEventAsync())
                                    .Returns(Task.FromResult<GetNextEventClientResponse?>(new GetNextEventClientResponse()
                                    {
                                        Event = event_
                                    }));
            eventServiceClientMock.Setup(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()));
            eventDispatcherServiceClientMock.Setup(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()))
                                    .Returns((DispatchEventClientRequest request) =>
                                    {
                                        return Task.FromResult<DispatchEventClientResponse?>(new DispatchEventClientResponse()
                                        {
                                            ReceivedEventId = "ReceivedEevntId"
                                        });
                                    });
            IEventDispatchWorker eventDispatchWorker = new EventDispatchWorker(_loggerMock.Object, eventServiceClientMock.Object, eventDispatcherServiceClientMock.Object, workerConfigOptions, _appTerminatorMock.Object);

            await eventDispatchWorker.Run();

            eventServiceClientMock.Verify(s => s.GetNextEventAsync(), Times.Once());
            eventDispatcherServiceClientMock.Verify(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()), Times.Never());
            eventServiceClientMock.Verify(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()), Times.Never());
        }

        [Fact]
        public async Task Run_WitOnlyMissingSubscribers()
        {
            Mock<IEventServiceClient> eventServiceClientMock = new Mock<IEventServiceClient>();
            Mock<IEventDispatcherServiceClient> eventDispatcherServiceClientMock = new Mock<IEventDispatcherServiceClient>();
            IOptions<WorkerConfigOptions> workerConfigOptions = Options.Create<WorkerConfigOptions>(new WorkerConfigOptions()
            {
                InfiniteRun = false
            });
            Event event_ = new Event()
            {
                Id = "EventId",
                Name = EventNames.Test,
                Source = EventSources.Test,
                Consumers = new List<EventConsumer>
                {
                    new EventConsumer(){ ApplicationName = ApplicationNames.ProductService }
                },
                Subscribers = new List<EventSubscriber>
                {
                    new EventSubscriber(){ ApplicationName = ApplicationNames.ProductService },
                    new EventSubscriber(){ ApplicationName = ApplicationNames.EventOrchestrator }
                }
            };
            eventServiceClientMock.Setup(s => s.GetNextEventAsync())
                                    .Returns(Task.FromResult<GetNextEventClientResponse?>(new GetNextEventClientResponse()
                                    {
                                        Event = event_
                                    }));
            eventServiceClientMock.Setup(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()));
            eventDispatcherServiceClientMock.Setup(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()))
                                    .Returns((DispatchEventClientRequest request) =>
                                    {
                                        return Task.FromResult<DispatchEventClientResponse?>(new DispatchEventClientResponse()
                                        {
                                            ReceivedEventId = "ReceivedEevntId"
                                        });
                                    });
            IEventDispatchWorker eventDispatchWorker = new EventDispatchWorker(_loggerMock.Object, eventServiceClientMock.Object, eventDispatcherServiceClientMock.Object, workerConfigOptions, _appTerminatorMock.Object);

            await eventDispatchWorker.Run();

            eventServiceClientMock.Verify(s => s.GetNextEventAsync(), Times.Once());
            eventDispatcherServiceClientMock.Verify(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()), Times.Once());
            eventServiceClientMock.Verify(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()), Times.Once());
        }

        [Fact]
        public async Task Run_WhenFailToDispatchEvent_StillConsumeEventAtTheEnd()
        {
            Mock<IEventServiceClient> eventServiceClientMock = new Mock<IEventServiceClient>();
            Mock<IEventDispatcherServiceClient> eventDispatcherServiceClientMock = new Mock<IEventDispatcherServiceClient>();
            IOptions<WorkerConfigOptions> workerConfigOptions = Options.Create<WorkerConfigOptions>(new WorkerConfigOptions()
            {
                InfiniteRun = false
            });
            Event event_ = new Event()
            {
                Id = "EventId",
                Name = EventNames.Test,
                Source = EventSources.Test,
                Consumers = new List<EventConsumer>(),
                Subscribers = new List<EventSubscriber>
                {
                    new EventSubscriber(){ ApplicationName= ApplicationNames.ProductService },
                    new EventSubscriber(){ ApplicationName= ApplicationNames.Aggregator }
                }
            };
            eventServiceClientMock.Setup(s => s.GetNextEventAsync())
                                    .Returns(Task.FromResult<GetNextEventClientResponse?>(new GetNextEventClientResponse()
                                    {
                                        Event = event_
                                    }));
            eventServiceClientMock.Setup(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()));
            eventDispatcherServiceClientMock.Setup(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()))
                                    .Returns((DispatchEventClientRequest request) =>
                                    {
                                        return Task.FromResult<DispatchEventClientResponse?>(null);
                                    });
            IEventDispatchWorker eventDispatchWorker = new EventDispatchWorker(_loggerMock.Object, eventServiceClientMock.Object, eventDispatcherServiceClientMock.Object, workerConfigOptions, _appTerminatorMock.Object);

            await eventDispatchWorker.Run();

            eventServiceClientMock.Verify(s => s.GetNextEventAsync(), Times.Once());
            eventDispatcherServiceClientMock.Verify(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()), Times.Exactly(2));
            eventServiceClientMock.Verify(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Run_RetryDispatching()
        {
            Mock<IEventServiceClient> eventServiceClientMock = new Mock<IEventServiceClient>();
            Mock<IEventDispatcherServiceClient> eventDispatcherServiceClientMock = new Mock<IEventDispatcherServiceClient>();
            IOptions<WorkerConfigOptions> workerConfigOptions = Options.Create<WorkerConfigOptions>(new WorkerConfigOptions()
            {
                InfiniteRun = false,
                DispatchRetryCountBeforeForcingConsume = 3,
                RetryWaitInSeconds = 0
            });
            Event event_ = new Event()
            {
                Id = "EventId",
                Name = EventNames.Test,
                Source = EventSources.Test,
                Consumers = new List<EventConsumer>(),
                Subscribers = new List<EventSubscriber>
                {
                    new EventSubscriber(){ ApplicationName= ApplicationNames.ProductService },
                    new EventSubscriber(){ ApplicationName= ApplicationNames.Aggregator }
                }
            };
            eventServiceClientMock.Setup(s => s.GetNextEventAsync())
                                    .Returns(Task.FromResult<GetNextEventClientResponse?>(new GetNextEventClientResponse()
                                    {
                                        Event = event_
                                    }));
            eventServiceClientMock.Setup(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()));
            eventDispatcherServiceClientMock.Setup(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()))
                                    .Throws(new System.Exception("Any exception"));
            IEventDispatchWorker eventDispatchWorker = new EventDispatchWorker(_loggerMock.Object, eventServiceClientMock.Object, eventDispatcherServiceClientMock.Object, workerConfigOptions, _appTerminatorMock.Object);

            await eventDispatchWorker.Run();

            eventServiceClientMock.Verify(s => s.GetNextEventAsync(), Times.Once());
            eventDispatcherServiceClientMock.Verify(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()), Times.Exactly(2 * workerConfigOptions.Value.DispatchRetryCountBeforeForcingConsume));
            eventServiceClientMock.Verify(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Run_ExitIfFailedToConsume()
        {
            Mock<IEventServiceClient> eventServiceClientMock = new Mock<IEventServiceClient>();
            Mock<IEventDispatcherServiceClient> eventDispatcherServiceClientMock = new Mock<IEventDispatcherServiceClient>();
            IOptions<WorkerConfigOptions> workerConfigOptions = Options.Create<WorkerConfigOptions>(new WorkerConfigOptions()
            {
                InfiniteRun = false,
                RetryWaitInSeconds = 0
            });
            Event event_ = new Event()
            {
                Id = "EventId",
                Name = EventNames.Test,
                Source = EventSources.Test,
                Consumers = new List<EventConsumer>(),
                Subscribers = new List<EventSubscriber>
                {
                    new EventSubscriber(){ ApplicationName= ApplicationNames.ProductService },
                    new EventSubscriber(){ ApplicationName= ApplicationNames.Aggregator }
                }
            };
            eventServiceClientMock.Setup(s => s.GetNextEventAsync())
                                    .Returns(Task.FromResult<GetNextEventClientResponse?>(new GetNextEventClientResponse()
                                    {
                                        Event = event_
                                    }));
            eventServiceClientMock.Setup(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()))
                                    .Throws(new System.Exception("Any exception"));
            eventDispatcherServiceClientMock.Setup(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()))
                                    .Throws(new System.Exception("Any exception"));
            _appTerminatorMock.Reset();
            _appTerminatorMock.Setup(s => s.Terminate(It.IsAny<int>()));
            IEventDispatchWorker eventDispatchWorker = new EventDispatchWorker(_loggerMock.Object, eventServiceClientMock.Object, eventDispatcherServiceClientMock.Object, workerConfigOptions, _appTerminatorMock.Object);

            await eventDispatchWorker.Run();

            eventServiceClientMock.Verify(s => s.GetNextEventAsync(), Times.Once());
            eventDispatcherServiceClientMock.Verify(s => s.DispatchEventAsync(It.IsAny<DispatchEventClientRequest>()), Times.Exactly(event_.Subscribers.Count * workerConfigOptions.Value.DispatchRetryCountBeforeForcingConsume));
            // Unfortunately, I don't know yet how to stop the unit test execution.
            // That's why, it would run as many times as we have subscribers
            eventServiceClientMock.Verify(s => s.ConsumeEventAsync(It.IsAny<ConsumeEventClientRequest>()), Times.Exactly(event_.Subscribers.Count * EventDispatchWorker.RetryCountBeforeFatal));
            _appTerminatorMock.Verify(s => s.Terminate(EventDispatchWorker.FatalExitCode), Times.Exactly(event_.Subscribers.Count));
        }
    }
}
