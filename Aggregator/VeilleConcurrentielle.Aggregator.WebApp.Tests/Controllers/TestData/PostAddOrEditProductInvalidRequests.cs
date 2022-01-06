extern alias mywebapp;

using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Models;
using System.Collections;
using System.Collections.Generic;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers.TestData
{
    internal class PostAddOrEditProductInvalidRequests : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // missing ProductName
            yield return new object[]
            {
                new AddOrEditProductModels.AddOrEditProductRequest()
                {
                    ProductId = "ProductId",
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
                                        new ConfigHolder.ConfigItem()
                                        {
                                            Key = ConfigHolderKeys.ProductPageUrl.ToString(),
                                            Value = "https://anyurl.com"
                                        }
                                    }
                                }
                            }
                        }
                }
            };

            // missing Strategies
            yield return new object[]
            {
                new AddOrEditProductModels.AddOrEditProductRequest()
                {
                    ProductId = "ProductId",
                    ProductName = "ProductName",
                    Price = 100,
                    Quantity = 10,
                    Strategies = null,
                    CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>
                        {
                            new AddOrUPdateProductRequestedEventPayload.CompetitorConfig(){
                                CompetitorId= CompetitorIds.ShopA,
                                Holder = new ConfigHolder()
                                {
                                    Items = new List<ConfigHolder.ConfigItem>
                                    {
                                        new ConfigHolder.ConfigItem()
                                        {
                                            Key = ConfigHolderKeys.ProductPageUrl.ToString(),
                                            Value = "https://anyurl.com"
                                        }
                                    }
                                }
                            }
                        }
                }
            };

            // missing 1 Strategy
            yield return new object[]
            {
                new AddOrEditProductModels.AddOrEditProductRequest()
                {
                    ProductId = "ProductId",
                    ProductName = "ProductName",
                    Price = 100,
                    Quantity = 10,
                    Strategies = new List<AddOrUPdateProductRequestedEventPayload.Strategy>(),
                    CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>
                        {
                            new AddOrUPdateProductRequestedEventPayload.CompetitorConfig(){
                                CompetitorId = CompetitorIds.ShopA,
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
                }
            };

            // missing CompetitorConfigs
            yield return new object[]
            {
                new AddOrEditProductModels.AddOrEditProductRequest()
                {
                    ProductId = "ProductId",
                    ProductName = "ProductName",
                    Price = 100,
                    Quantity = 10,
                    Strategies = new List<AddOrUPdateProductRequestedEventPayload.Strategy> {
                            new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallAveragePrice},
                            new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallCheaperPrice}
                        },
                    CompetitorConfigs = null
                }
            };

            // missing 1 CompetitorConfigs
            yield return new object[]
            {
                new AddOrEditProductModels.AddOrEditProductRequest()
                {
                    ProductId = "ProductId",
                    ProductName = "ProductName",
                    Price = 100,
                    Quantity = 10,
                    Strategies = new List<AddOrUPdateProductRequestedEventPayload.Strategy> {
                            new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallAveragePrice},
                            new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallCheaperPrice}
                        },
                    CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>()
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
