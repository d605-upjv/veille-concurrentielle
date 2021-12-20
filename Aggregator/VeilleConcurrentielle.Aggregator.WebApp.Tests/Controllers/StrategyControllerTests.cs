using Xunit;
using System.Net.Http.Json;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using System.Collections.Generic;
using System;
using VeilleConcurrentielle.Aggregator.Lib.Contracts;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers
{
    public class StrategyControllerTests
    {
        [Fact]
        public async void GetAll()
        {
            await using var application = new WebApp();
            using var client = application.CreateClient();
            var response = await client.GetFromJsonAsync<List<CompetitorDto>>("/api/Strategys");
            Assert.NotNull(response);
            var knownCompetitors = Enum.GetValues<StrategyIds>();
            Assert.True(knownCompetitors.Length == response.Count, $"Make sure that all strategies are seeded properly! (current: {response.Count}, expected: {knownCompetitors.Length})");
        }
    }
}
