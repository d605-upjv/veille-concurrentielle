using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Core.Configurations;
using VeilleConcurrentielle.ProductService.Lib.Clients.ServiceClients;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Clients.ServiceClients
{
    public class ProductServiceClientTests
    {
        private readonly Mock<ILogger<ProductServiceClient>> _loggerMock;
        public ProductServiceClientTests()
        {
            _loggerMock = new Mock<ILogger<ProductServiceClient>>();
        }

        [Fact]
        public async Task GetProductsToScrapAsync_Integration()
        {
            await using var application = new ProductWebApp();
            using var httpClient = application.CreateClient();

            IOptions<ServiceUrlsOptions> serviceUrlOptions = Options.Create(new ServiceUrlsOptions()
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                ProductUrl = httpClient.BaseAddress.ToString()
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            });
            IProductServiceClient productServiceClient = new ProductServiceClient(httpClient, serviceUrlOptions, _loggerMock.Object);
            var response = await productServiceClient.GetProductsToScrapAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.Products);
        }
    }
}
