extern alias mywebapp;

using System.Net.Http.Json;
using VeilleConcurrentielle.ProductService.Lib.Servers.Models;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async void GetProductsToScrap_Integration()
        {
            await using var application = new ProductWebApp();
            using var client = application.CreateClient();
            var response = await client.GetFromJsonAsync<GetProductsToScrapServerResponse>("/api/Products/scrap");
            Assert.NotNull(response);
            Assert.NotNull(response.Products);
        }
    }
}
