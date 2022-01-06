extern alias mywebapp;

using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Models;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers
{
    public class MainShopControllerTests
    {
        [Fact]
        public async void GetMainShopProductAsync_Integration()
        {
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var response = await client.GetAsync("/api/MainShop?productUrl=https://anyurl");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async void voidUpdateProductPriceAsync_Integration()
        {
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var response = await client.PostAsJsonAsync<UpdateMainShopProductPriceModels.UpdaetMainShopProductPriceRequest>("api/MainShop/price", new UpdateMainShopProductPriceModels.UpdaetMainShopProductPriceRequest()
            {
                ProductId = "ProductId",
                Price = 10
            });
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
