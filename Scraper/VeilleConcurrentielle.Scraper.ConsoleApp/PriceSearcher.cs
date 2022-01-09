using Microsoft.Extensions.Logging;

namespace VeilleConcurrentielle.Scraper.ConsoleApp
{
    public class PriceSearcher : IPriceSearcher
    {
        private readonly ILogger<PriceSearcher> _logger;
        private readonly IHtmlDocumentLoader _htmlDocumentLoader;
        public PriceSearcher(ILogger<PriceSearcher> logger, IHtmlDocumentLoader htmlDocumentLoader)
        {
            _logger = logger;
            _htmlDocumentLoader = htmlDocumentLoader;
        }

        public double? FindPrice(string url, string xPath)
        {
            try
            {
                var htmlDoc = _htmlDocumentLoader.GetHtmlDocument(url);
                var node = htmlDoc.DocumentNode.SelectSingleNode(xPath);
                if (node != null)
                {
                    var rawValue = node.InnerText;
                    if (!string.IsNullOrWhiteSpace(rawValue))
                    {
                        var onlyValidCharsValue = String.Join("", rawValue.Where(e => Char.IsDigit(e) || e == ',' || e == '.'))
                                                        .Replace(".", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                                                        .Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                        if (double.TryParse(onlyValidCharsValue, out double price))
                            return price;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to scrap price in the url {url} (CSS class: {xPath})");
            }
            return null;
        }
    }
}
