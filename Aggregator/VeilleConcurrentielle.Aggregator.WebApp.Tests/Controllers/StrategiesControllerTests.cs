extern alias mywebapp;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Models;
using System;
using System.Net.Http.Json;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers
{
    public class StrategiesControllerTests
    {
        [Fact]
        public async void GetAll_Integration()
        {
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var response = await client.GetFromJsonAsync<GetAllStrategiessModels.GetAllStrategiesResponse>("/api/Strategies");
            Assert.NotNull(response);
            Assert.NotNull(response.Strategies);
            var knownCompetitors = Enum.GetValues<StrategyIds>();
            Assert.True(knownCompetitors.Length == response.Strategies.Count, $"Make sure that all strategies are seeded properly! (current: {response.Strategies.Count}, expected: {knownCompetitors.Length})");
        }
    }
}
