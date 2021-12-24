
namespace VeilleConcurrentielle.EventOrchestrator.ConsoleApp
{
    public interface IEventDispatchWorker
    {
        Task Run();
    }
}