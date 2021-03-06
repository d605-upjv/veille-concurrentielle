extern alias mywebapp;

using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services.Recommendations;
using System.Collections.Generic;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Core.Services.Recommendations
{
    public class OverallCheaperPriceEngineTests : RecommendationEngineTests
    {
        [Fact]
        public void GetRecommendedPrice()
        {
            double productPrice = 100;
            Dictionary<CompetitorIds, double> priceByCompetitors = new Dictionary<CompetitorIds, double>()
            {
                { CompetitorIds.ShopA, productPrice + 1 },
                { CompetitorIds.ShopB, productPrice + 2 }
            };
            RecommendationEngine engine = new OverallCheaperPriceEngine();
            var lastCompetitorPrices = GenerateLatestProductPrices(priceByCompetitors);
            var recommendedPrice = engine.GetRecommendedPrice(productPrice, 10, lastCompetitorPrices);
            Assert.Equal(productPrice, recommendedPrice);
        }
    }
}
