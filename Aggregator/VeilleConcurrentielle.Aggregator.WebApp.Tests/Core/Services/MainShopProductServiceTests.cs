extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Core.Models;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Core.Services;
using System.Threading.Tasks;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Core.Services
{
    public class MainShopProductServiceTests
    {
        private readonly Mock<IMainShopWebService> _mainShopWebServiceMock;
        public MainShopProductServiceTests()
        {
            _mainShopWebServiceMock = new Mock<IMainShopWebService>();
        }
        [Theory]
        [InlineData("http://main-shop.d605-shops.fr/fr/men/1-1-hummingbird-printed-t-shirt.html#/1-taille-s/8-couleur-blanc", "1")]
        [InlineData("http://main-shop.d605-shops.fr/fr/home-accessories/7-mug-the-adventure-begins.html", "7")]
        [InlineData("http://main-shop.d605-shops.fr/en/home-accessories/7-mug-the-adventure-begins.html", "7")]
        [InlineData("http://main-shop.d605-shops.fr/152", "152")]
        public void GetProductId_ValidUrl(string productUrl, string expectedProductId)
        {
            IMainShopProductService productService = new MainShopProductService(_mainShopWebServiceMock.Object);
            var productId = productService.GetProductId(productUrl);
            Assert.Equal(expectedProductId, productId);
        }

        [Theory]
        [InlineData("invalidurl")]
        [InlineData("http://main-shop.d605-shops.fr/en/home-accessories/x7-mug-the-adventure-begins.html")]
        public void GetProductId_InvalidUrl(string productUrl)
        {
            IMainShopProductService productService = new MainShopProductService(_mainShopWebServiceMock.Object);
            var productId = productService.GetProductId(productUrl);
            Assert.Null(productId);
        }

        [Fact]
        public async Task GetProductAsync()
        {
            string productUrl = "https://anyurl/1-anything";
            string productId = "1";
            _mainShopWebServiceMock.Setup(s => s.GetProductAsync(It.IsAny<string>(), It.IsAny<string>()))
                                        .Returns(Task.FromResult<MainShopProduct?>(new MainShopProduct()
                                        {
                                            ShopProductUrl = productUrl,
                                            ShopProductId = productId
                                        }));
            IMainShopProductService productService = new MainShopProductService(_mainShopWebServiceMock.Object);
            var product = await productService.GetProductAsync(productUrl);
            Assert.NotNull(product);
            Assert.Equal(productUrl, product.ShopProductUrl);
            Assert.Equal(productId, product.ShopProductId);
            _mainShopWebServiceMock.Verify(s => s.GetProductAsync(productId, productUrl), Times.Once());
        }
    }
}
