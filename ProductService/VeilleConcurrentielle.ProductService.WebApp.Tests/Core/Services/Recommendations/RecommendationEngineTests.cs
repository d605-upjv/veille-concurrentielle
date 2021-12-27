using System;
using System.Collections.Generic;
using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests.Core.Services.Recommendations
{
    public abstract class RecommendationEngineTests
    {
        protected CompetitorProductPrices GenerateLatestProductPrices(Dictionary<CompetitorIds, double> priceByCompetitors)
        {
            CompetitorProductPrices lastCompetitorPrices = new CompetitorProductPrices();
            lastCompetitorPrices.Prices = new List<CompetitorProductPrices.CompetitorItemProductPrices>();
            var competitorIds = Enum.GetValues<CompetitorIds>();
            Random random = new Random();
            foreach(var price in priceByCompetitors)
            {
                lastCompetitorPrices.Prices.Add(new CompetitorProductPrices.CompetitorItemProductPrices()
                {
                    CompetitorId = price.Key,
                    Prices = new List<ProductPrice>
                    {
                        new ProductPrice()
                        {
                            Price = price.Value,
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
