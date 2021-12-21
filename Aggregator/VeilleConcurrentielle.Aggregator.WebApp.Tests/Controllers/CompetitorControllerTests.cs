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
            var response = await client.GetFromJsonAsync<GetAllCompetitorsModels.GetAllCompetitorResponse>("/api/Competitors");
            Assert.NotNull(response);
            Assert.NotNull(response.Competitors);
            var knownCompetitors = Enum.GetValues<CompetitorIds>();
            Assert.True(knownCompetitors.Length == response.Competitors.Count, $"Make sure that all competitors are seeded properly! (current: {response.Competitors.Count}, expected: {knownCompetitors.Length})");
        }
    }
}
