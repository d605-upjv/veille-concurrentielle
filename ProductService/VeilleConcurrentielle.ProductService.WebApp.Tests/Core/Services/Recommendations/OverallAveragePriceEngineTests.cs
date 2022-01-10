extern alias mywebapp;
using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Core.Services.Recommendations;
using System;
using System.Collections.Generic;
using System.Linq;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using Xunit;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Core.Services.Recommendations
{
    public class OverallAveragePriceEngineTests : RecommendationEngineTests
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
            RecommendationEngine engine = new OverallAveragePriceEngine();
            var lastCompetitorPrices = GenerateLatestProductPrices(priceByCompetitors);
            var recommendedPrice = engine.GetRecommendedPrice(productPrice, 10, lastCompetitorPrices);
            var expectedPrice = Math.Round((new double[] { productPrice, productPrice + 1, productPrice + 2 }).Average(), 2);
            Assert.Equal(expectedPrice, recommendedPrice);
        }
    }
}
