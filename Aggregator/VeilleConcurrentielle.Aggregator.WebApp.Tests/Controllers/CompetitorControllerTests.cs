using Xunit;
using System.Net.Http.Json;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using System.Collections.Generic;
using System;
using VeilleConcurrentielle.Aggregator.Lib.Contracts;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers
{
    public class CompetitorControllerTests
    {
        [Fact]
        public async void GetAll()
        {
            await using var application = new WebApp();
            using var client = application.CreateClient();
            var response = await client.GetFromJsonAsync<List<CompetitorDto>>("/api/Competitors");
            Assert.NotNull(response);
            var knownCompetitors = Enum.GetValues<CompetitorIds>();
            Assert.True(knownCompetitors.Length == response.Count, $"Make sure that all competitors are seeded properly! (current: {response.Count}, expected: {knownCompetitors.Length})");
        }
    }
}
