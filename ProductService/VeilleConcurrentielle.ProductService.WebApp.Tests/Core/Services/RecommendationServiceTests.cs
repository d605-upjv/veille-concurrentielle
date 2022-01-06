extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Models;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Core.Services
{
    public class RecommendationServiceTests
    {
        private readonly Mock<IRecommendationRepository> _recommendationRepositoryMock;
        public RecommendationServiceTests()
        {
            _recommendationRepositoryMock = new Mock<IRecommendationRepository>();
        }

        [Fact]
        public void GetRecommendationEngine_ScopesAllStrategies()
        {
            IRecommendationService recommendationService = new RecommendationService(_recommendationRepositoryMock.Object);
            StrategyIds[] strategies = Enum.GetValues<StrategyIds>();
            foreach(var strategyId in strategies)
            {
                var engine = recommendationService.GetRecommendationEngine(strategyId);
                Assert.NotNull(engine);
            }
        }

        [Fact]
        public async Task GetRecommendationsAsync_ReturnsAsMuchRecommendationsAsStrategiesIfDifferentPrices()
        {
            string productId = "ProductId";
            double price = 100;
            int quantity = 10;
            StrategyIds[] strategies = Enum.GetValues<StrategyIds>();
            StrategyIds[] expectedStrategiesWithREcommendations = { StrategyIds.OverallAveragePrice, StrategyIds.FivePercentAboveMeanPrice };
            IRecommendationService recommendationService = new RecommendationService(_recommendationRepositoryMock.Object);
            CompetitorProductPrices lastCompetitorPrices = GenerateLatestProductPrices(Convert.ToInt32(price + 1));
            var recommendations = await recommendationService.GetRecommendationsAsync(new GetRecommendationRequest()
            {
                ProductId = productId,
                Price = price,
                Quantity = quantity,
                Strategies = strategies.ToList(),
                LastCompetitorPrices = lastCompetitorPrices
            });

            Assert.NotNull(recommendations);
            Assert.NotEmpty(recommendations.Recommendations);
            Assert.NotEmpty(recommendations.NewRecommendations);
            Assert.Equal(expectedStrategiesWithREcommendations.Length, recommendations.Recommendations.Count);
            Assert.Equal(expectedStrategiesWithREcommendations.Length, recommendations.NewRecommendations.Count);
            _recommendationRepositoryMock.Verify(s => s.GetLatestRecommendationAsync(productId, It.IsAny<string>()), Times.Exactly(strategies.Length));
        }

        [Fact]
        public async Task GetRecommendationsAsync_NewRecommendations()
        {
            string productId = "ProductId";
            double price = 100;
            int quantity = 10;
            StrategyIds[] strategies = { StrategyIds.OverallCheaperPrice, StrategyIds.OverallAveragePrice };
            IRecommendationService recommendationService = new RecommendationService(_recommendationRepositoryMock.Object);
            CompetitorProductPrices lastCompetitorPrices = GenerateLatestProductPrices(Convert.ToInt32(price));
            var competitorAPrice = lastCompetitorPrices.Prices.First();
            competitorAPrice.Prices[0].Price = 100 - 1;
            _recommendationRepositoryMock.Setup(s => s.GetLatestRecommendationAsync(productId, StrategyIds.OverallCheaperPrice.ToString()))
                                                            .Returns(Task.FromResult<RecommendationEntity?>(new RecommendationEntity()
                                                            {
                                                                Id = "RecoId",
                                                                Price = price - 1
                                                            }));
            var recommendations = await recommendationService.GetRecommendationsAsync(new GetRecommendationRequest()
            {
                ProductId = productId,
                Price = price,
                Quantity = quantity,
                Strategies = strategies.ToList(),
                LastCompetitorPrices = lastCompetitorPrices
            });

            var newRecommendation = recommendations.Recommendations.Where(r => r.StrategyId == StrategyIds.OverallAveragePrice).First();
            Assert.NotNull(recommendations);
            Assert.NotEmpty(recommendations.Recommendations);
            Assert.NotEmpty(recommendations.NewRecommendations);
            Assert.Equal(strategies.Length, recommendations.Recommendations.Count);
            Assert.Single(recommendations.NewRecommendations);
            Assert.Equal(newRecommendation.Id, recommendations.NewRecommendations.First().Id);
            _recommendationRepositoryMock.Verify(s => s.GetLatestRecommendationAsync(productId, It.IsAny<string>()), Times.Exactly(strategies.Length));
        }

        private CompetitorProductPrices GenerateLatestProductPrices(int lowestPrice = 10, int highestPrice = 1000)
        {
            CompetitorProductPrices lastCompetitorPrices = new CompetitorProductPrices();
            lastCompetitorPrices.Prices = new List<CompetitorProductPrices.CompetitorItemProductPrices>();
            var competitorIds = Enum.GetValues<CompetitorIds>();
            Random random = new Random();
            foreach (var competitorId in competitorIds)
            {
                lastCompetitorPrices.Prices.Add(new CompetitorProductPrices.CompetitorItemProductPrices()
                {
                    CompetitorId = competitorId,
                    Prices = new List<ProductPrice>
                    {
                        new ProductPrice()
                        {
                            Price = random.Next(lowestPrice, highestPrice),
                            Quantity = 10,
                            CreatedAt = DateTime.Now
                        }
                    }
                });
            }
            return lastCompetitorPrices;
        }
    }
}
