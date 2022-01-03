extern alias mywebapp;

using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Models;
using System.Net;
using VeilleConcurrentielle.Infrastructure.Framework;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers
{
    public class RecommendationAlertControllerTests
    {
        [Fact]
        public async void GetAllUnseenAsync_Integration()
        {
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var response = await client.GetAsync("/api/RecommendationAlerts");
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<GetAllUnseenRecommendationAlertModels.GetAllUnseenRecommendationAlertResponse>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent);
            Assert.NotNull(responseContent.Recommendations);
        }

        [Fact]
        public async void SetToSeenAsync_Integration()
        {
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var response = await client.PutAsync("/api/RecommendationAlerts/seen/AlertId", null);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async void GetAllUnseenCountAsync_Integration()
        {
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var response = await client.GetAsync("/api/RecommendationAlerts/count");
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<GetAllUnseenRecommendationAlertCountModels.GetAllUnseenRecommendationAlertCountResponse>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent);
            Assert.Equal(0, responseContent.Count);
        }

        [Fact]
        public async void GetAllUnseenCountByProductAsync_Integration()
        {
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var response = await client.GetAsync("/api/RecommendationAlerts/products/count");
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<GetAllUnseenRecommendationAlertCountByProductModels.GetAllUnseenRecommendationAlertCountByProductResponse>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent);
            Assert.NotNull(responseContent.Items);
            Assert.Empty(responseContent.Items);
        }

        [Fact]
        public async void SetToSeenForProductAsync_Integration()
        {
            string productId = "ProductId";
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var response = await client.PostAsync($"/api/RecommendationAlerts/products/{productId}/seen", null);
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<SetRecommendationAlertForProductToSeenModels.SetRecommendationAlertForProductToSeenResponse>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent);
            Assert.Equal(0, responseContent.AffectedCount);
        }
    }
}
