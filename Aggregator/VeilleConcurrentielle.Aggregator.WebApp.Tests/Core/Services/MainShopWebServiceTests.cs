extern alias mywebapp;

using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Core.Configurations;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Core.Services;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Core.Services
{
    public class MainShopWebServiceTests
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly IOptions<MainShopOptions> _mainShopOptions;
        public MainShopWebServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _mainShopOptions = Options.Create<MainShopOptions>(new MainShopOptions()
            {
                ProductUrl = "http://someapi.com/{productId}"
            });
        }

        [Fact]
        public void CleanCDataForUpdate()
        {
            IMainShopWebService webService = new MainShopWebService(_httpClient, _mainShopOptions);
            var getProductData = File.ReadAllText(@"Core/Services/TestData/GET_Product_data.xml");
            var cleansed = webService.CleanCDataForUpdate(getProductData);
            Assert.NotNull(cleansed);
            Assert.True(cleansed.Contains("notFilterable") == false);
        }

        [Fact]
        public async Task GetProductAsync()
        {
            string productId = "1";
            string productUrl = "anyurl";
            var getProductData = File.ReadAllText(@"Core/Services/TestData/GET_Product_data.xml");
            var getImageData = File.ReadAllBytes(@"Core/Services/TestData/GET_image.jpg");
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    if (request.RequestUri.PathAndQuery.Contains("images"))
                    {
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new ByteArrayContent(getImageData)
                        };
                    }
                    else
                    {
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(getProductData)
                        };
                    }
                });
            IMainShopWebService webService = new MainShopWebService(_httpClient, _mainShopOptions);
            var product = await webService.GetProductAsync(productId, productUrl);
            Assert.NotNull(product);
            Assert.Equal(productId, product.ShopProductId);
            Assert.Equal(productUrl, product.ShopProductUrl);
            Assert.Equal("Hummingbird printed t-shirt", product.Name);
            Assert.Equal(23.9, product.Price);
            Assert.NotNull(product.ImageUrl);
        }

        [Fact]
        public async Task UpdateProductPriceAsync()
        {
            string productId = "1";
            double newPrice = 10;
            var getProductData = File.ReadAllText(@"Core/Services/TestData/GET_Product_data.xml");
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    if (request.Method == HttpMethod.Post)
                    {
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK
                        };
                    }
                    else
                    {
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(getProductData)
                        };
                    }
                });
            IMainShopWebService webService = new MainShopWebService(_httpClient, _mainShopOptions);
            var updatedProductId = await webService.UpdateProductPriceAsync(productId, newPrice);
            Assert.NotNull(updatedProductId);
            Assert.Equal(productId, updatedProductId);
        }
    }
}
