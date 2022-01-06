namespace VeilleConcurrentielle.Scraper.ConsoleApp
{
    public interface IAppTerminator
    {
        void Terminate(int exitCode);
    }
}