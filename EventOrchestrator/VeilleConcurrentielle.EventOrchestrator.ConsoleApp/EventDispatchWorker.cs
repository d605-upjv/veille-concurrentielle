using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Framework;

namespace VeilleConcurrentielle.EventOrchestrator.ConsoleApp
{
    public class EventDispatchWorker : IEventDispatchWorker
    {
        public const int RetryCountBeforeFatal = 3;
        public const int FatalExitCode = 99;
        private readonly ILogger<EventDispatchWorker> _logger;
        private readonly IEventServiceClient _eventServiceClient;
        private readonly IEventDispatcherServiceClient _eventDispatcherServiceClient;
        private readonly WorkerConfigOptions _workerConfig;
        private readonly IAppTerminator _appTerminator;
        public EventDispatchWorker(ILogger<EventDispatchWorker> logger, IEventServiceClient eventServiceClient, IEventDispatcherServiceClient eventDispatcherServiceClient, IOptions<WorkerConfigOptions> workerConfigOptions, IAppTerminator appTerminator)
        {
            _logger = logger;
            _eventServiceClient = eventServiceClient;
            _eventDispatcherServiceClient = eventDispatcherServiceClient;
            _workerConfig = workerConfigOptions.Value;
            _appTerminator = appTerminator;
        }
        public async Task Run()
        {
            _logger.LogInformation("Start dispatching events");
            bool errorOccuredPreviously = false;
            while (true)
            {
                GetNextEventServerResponse? eventToProcess = null;
                try
                {
                    eventToProcess = await _eventServiceClient.GetNextEventAsync();
                    if (errorOccuredPreviously)
                    {
                        _logger.LogInformation($"Get next event is up again");
                        errorOccuredPreviously = false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed getting next event");
                    errorOccuredPreviously = true;
                }
                if (eventToProcess != null)
                {
                    var eventStr = SerializationUtils.Serialize(eventToProcess);
                    _logger.LogInformation($"New event to process: {eventToProcess.Event.Name}\nEvent: {eventStr}");
                    await Parallel.ForEachAsync(eventToProcess.Event.Subscribers, async (subscriber, cancellationToken) =>
                    {
                        if (!eventToProcess.Event.Consumers.Exists(c => c.ApplicationName == subscriber.ApplicationName))
                        {
                            await DispatchEvent(eventToProcess.Event, subscriber.ApplicationName);
                        }
                    });
                }
                if (!_workerConfig.InfiniteRun)
                {
                    break;
                }
                Thread.Sleep(_workerConfig.GetNextWaitInSeconds * 1000);
            }
        }

        private async Task DispatchEvent(Event event_, ApplicationNames applicationName)
        {
            _logger.LogInformation($"Dispatch event {event_.Name} to {applicationName}");
            DispatchEventClientRequest request = new DispatchEventClientRequest();
            request.EventId = event_.Id;
            request.ApplicationName = applicationName;
            request.Source = event_.Source;
            request.DispatchedAt = DateTime.Now;
            request.CreatedAt = event_.CreatedAt;
            request.EventName = event_.Name;
            request.SerializedPayload = event_.SerializedPayload;
            var requestStr = SerializationUtils.Serialize(request);
            string? dispatchErorMessage = null;
            var dispatchPolicy = Policy
                                    .Handle<Exception>()
                                    .WaitAndRetryAsync(_workerConfig.DispatchRetryCountBeforeForcingConsume - 1,
                                        retryAttempt => TimeSpan.FromSeconds(_workerConfig.RetryWaitInSeconds),
                                        async (ex, retryCount) =>
                                        {
                                            _logger.LogError(ex, $"Failed to dispatch event {event_.Name} ({event_.Id}) to {applicationName} (currenty retry: {retryCount})\nRequest: {requestStr}");
                                        });
            try
            {
                await dispatchPolicy.ExecuteAsync(async () =>
                {
                    var response = await _eventDispatcherServiceClient.DispatchEventAsync(request);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to dispatch event {event_.Name} ({event_.Id}) to {applicationName} (no more retries left)\nRequest: {requestStr}");
                dispatchErorMessage = ex.ToString();
            }

            var consumePolicy = Policy
                                    .Handle<Exception>()
                                    .WaitAndRetryAsync(RetryCountBeforeFatal - 1,
                                                        retryAttempt => TimeSpan.FromSeconds(_workerConfig.RetryWaitInSeconds),
                                                        onRetryAsync: async (ex, retryCount) =>
                                                         {
                                                             _logger.LogError(ex, $"Failed to consume event {event_.Name} ({event_.Id}) for application {applicationName}");
                                                         });
            try
            {
                await consumePolicy.ExecuteAsync(async () =>
                {
                    var consumeEventResponse = await _eventServiceClient.ConsumeEventAsync(new ConsumeEventClientRequest()
                    {
                        ApplicationName = applicationName,
                        EventId = event_.Id,
                        Reason = dispatchErorMessage
                    });
                    _logger.LogInformation($"Successfully consumed event {event_.Name} ({event_.Id}) for application {applicationName} with reason: {dispatchErorMessage}");
                });
            }catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Failed to consume event {event_.Name} ({event_.Id}) for application {applicationName}");
                _logger.LogCritical("Exit program due to fatal error");
                _appTerminator.Terminate(FatalExitCode);
            }
        }
    }
}
