namespace VeilleConcurrentielle.EventOrchestrator.ConsoleApp
{
    public class WorkerConfigOptions
    {
        public const string WorkerConfig = "WorkerConfig";
        public int GetNextWaitInSeconds { get; set; } = 5;
        public bool InfiniteRun { get; set; } = false;
        public int DispatchRetryCountBeforeForcingConsume { get; set; } = 1;
        public int RetryWaitInSeconds { get; set; } = 5;
    }
}
