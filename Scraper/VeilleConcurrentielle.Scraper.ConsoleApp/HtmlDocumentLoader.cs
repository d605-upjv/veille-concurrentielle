using HtmlAgilityPack;

namespace VeilleConcurrentielle.Scraper.ConsoleApp
{
    public class HtmlDocumentLoader : IHtmlDocumentLoader
    {
        public HtmlDocument GetHtmlDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);
            return htmlDoc;
        }
    }
}
