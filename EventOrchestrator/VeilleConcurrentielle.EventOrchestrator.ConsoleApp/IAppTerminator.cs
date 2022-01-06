namespace VeilleConcurrentielle.EventOrchestrator.ConsoleApp
{
    public interface IAppTerminator
    {
        void Terminate(int exitCode);
    }
}