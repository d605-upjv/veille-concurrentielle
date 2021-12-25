extern alias mywebapp;

using Xunit;
using System.Net.Http.Json;
using System;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers
{
    public class StrategyControllerTests
    {
        [Fact]
        public async void GetAll()
        {
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var response = await client.GetFromJsonAsync<GetAllStrategysModels.GetAllStrategysResponse>("/api/Strategys");
            Assert.NotNull(response);
            Assert.NotNull(response.Strategys);
            var knownCompetitors = Enum.GetValues<StrategyIds>();
            Assert.True(knownCompetitors.Length == response.Strategys.Count, $"Make sure that all strategies are seeded properly! (current: {response.Strategys.Count}, expected: {knownCompetitors.Length})");
        }
    }
}
