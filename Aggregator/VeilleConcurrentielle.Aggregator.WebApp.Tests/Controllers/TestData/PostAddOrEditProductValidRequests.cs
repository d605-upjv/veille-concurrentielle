extern alias mywebapp;

using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Models;
using System.Collections.Generic;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers.TestData
{
    static internal class PostAddOrEditProductValidRequests
    {
        public static AddOrEditProductModels.AddOrEditProductRequest GetStandardValidRequest()
        {
            return new AddOrEditProductModels.AddOrEditProductRequest()
            {
                ProductId = "Product1Id",
                ProductName = "Product1Name",
                ShopProductId = "1",
                Price = 100,
                Quantity = 10,
                Strategies = new List<AddOrUPdateProductRequestedEventPayload.Strategy> {
                    new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallAveragePrice},
                    new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallCheaperPrice}
                },
                CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>
                {
                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig(){
                        CompetitorId= CompetitorIds.ShopA,
                        Holder = new ConfigHolder()
                                {
                                    Items = new List<ConfigHolder.ConfigItem>
                                    {
                                        new ConfigHolder.ConfigItem
                                        {
                                            Key = ConfigHolderKeys.ProductPageUrl.ToString(),
                                            Value = "https://anyurl.com"
                                        }
                                    }
                                }
                    }
                }
            };
        }
    }
}
