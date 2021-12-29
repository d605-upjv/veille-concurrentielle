using System.Collections.Generic;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using Xunit;

namespace VeilleConcurrentielle.Infrastructure.Tests.Core.Models
{
    public class CompetitorProductPricesTests
    {
        [Fact]
        public void GetLatestPricesPerCompetitor_WithEmptyList()
        {
            CompetitorProductPrices competitorProductPrices = new CompetitorProductPrices()
            {
                Prices = new List<CompetitorProductPrices.CompetitorItemProductPrices>()
            };
            var prices = competitorProductPrices.GetLatestPricesPerCompetitor();
            Assert.Empty(prices);
        }

        [Fact]
        public void GetLatestPricesPerCompetitor_WithItems()
        {
            CompetitorProductPrices competitorProductPrices = new CompetitorProductPrices()
            {
                Prices = new List<CompetitorProductPrices.CompetitorItemProductPrices>
                {
                    new CompetitorProductPrices.CompetitorItemProductPrices()
                    {
                        Prices = new List<ProductPrice>()
                    }
                }
            };
            var prices = competitorProductPrices.GetLatestPricesPerCompetitor();
            Assert.Empty(prices);
        }
    }
}
