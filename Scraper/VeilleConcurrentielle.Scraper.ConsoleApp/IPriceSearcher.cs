namespace VeilleConcurrentielle.Scraper.ConsoleApp
{
    public interface IPriceSearcher
    {
        double? FindPrice(string url, string xPath);
    }
}