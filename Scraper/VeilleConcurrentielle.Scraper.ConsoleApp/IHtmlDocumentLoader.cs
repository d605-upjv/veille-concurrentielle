using HtmlAgilityPack;

namespace VeilleConcurrentielle.Scraper.ConsoleApp
{
    public interface IHtmlDocumentLoader
    {
        HtmlDocument GetHtmlDocument(string url);
    }
}