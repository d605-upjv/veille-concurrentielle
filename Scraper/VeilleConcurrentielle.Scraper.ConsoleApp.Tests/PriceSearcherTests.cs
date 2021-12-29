using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VeilleConcurrentielle.Scraper.ConsoleApp.Tests
{
    public class PriceSearcherTests
    {
        private readonly Mock<ILogger<PriceSearcher>> _logger;
        public PriceSearcherTests()
        {
            _logger = new Mock<ILogger<PriceSearcher>>();
        }

        [Fact]
        public async Task FindPrice_ParsePrestashop()
        {
            string xPath = "//span[contains(@class, 'current-price-value')]";
            Mock<IHtmlDocumentLoader> htmlDocumentLoaderMock = new Mock<IHtmlDocumentLoader>();
            string content = await File.ReadAllTextAsync("DataSamples/product_3_from_prestashop.html", Encoding.UTF8);
            htmlDocumentLoaderMock.Setup(s => s.GetHtmlDocument(It.IsAny<string>()))
                                    .Returns((string url) =>
                                    {
                                        var htmlDoc = new HtmlDocument();
                                        htmlDoc.LoadHtml(content);
                                        return htmlDoc;
                                    });
            IPriceSearcher priceSearcher = new PriceSearcher(_logger.Object, htmlDocumentLoaderMock.Object);
            var price = priceSearcher.FindPrice("anyurl", xPath);
            Assert.NotNull(price);
            Assert.Equal(29, price);
        }
    }
}
