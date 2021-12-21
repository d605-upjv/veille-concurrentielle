using System.Collections;
using System.Collections.Generic;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models.Events;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers.TestData
{
    internal class PostAddOrEditProductInvalidRequests : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new AddOrEditProductModels.AddOrEditProductRequest()
                {
                    ProductName = "ProductName",
                    Price = 100,
                    Quantity = 10,
                    Strategys = new List<AddOrUPdateProductRequestedEventPayload.Strategy> {
                            new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallAveragePrice},
                            new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallCheaperPrice}
                        },
                    CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>
                        {
                            new AddOrUPdateProductRequestedEventPayload.CompetitorConfig(){
                                CompetitorId= CompetitorIds.ShopA,
                                ProductExternalIds= new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId>
                                {
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId(){ Id="Unique1", Name=ProductExternalIdNames.UniqueId},
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId(){ Id="EAN1", Name=ProductExternalIdNames.EAN}
                                },
                                ProductUrls= new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductUrl>
                                {
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductUrl(){
                                        Name = ProductUrlNames.ProductProfile,
                                        Url = "https://product.com/Id1"
                                    }
                                }
                            }
                        }
                }
            };

            yield return new object[]
            {
                new AddOrEditProductModels.AddOrEditProductRequest()
                {
                    ProductId = "ProductId",
                    Price = 100,
                    Quantity = 10,
                    Strategys = new List<AddOrUPdateProductRequestedEventPayload.Strategy> {
                            new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallAveragePrice},
                            new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallCheaperPrice}
                        },
                    CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>
                        {
                            new AddOrUPdateProductRequestedEventPayload.CompetitorConfig(){
                                CompetitorId= CompetitorIds.ShopA,
                                ProductExternalIds= new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId>
                                {
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId(){ Id="Unique1", Name=ProductExternalIdNames.UniqueId},
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId(){ Id="EAN1", Name=ProductExternalIdNames.EAN}
                                },
                                ProductUrls= new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductUrl>
                                {
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductUrl(){
                                        Name = ProductUrlNames.ProductProfile,
                                        Url = "https://product.com/Id1"
                                    }
                                }
                            }
                        }
                }
            };

            yield return new object[]
            {
                new AddOrEditProductModels.AddOrEditProductRequest()
                {
                    ProductId = "ProductId",
                    ProductName = "ProductName",
                    Price = 100,
                    Quantity = 10,
                    Strategys = null,
                    CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>
                        {
                            new AddOrUPdateProductRequestedEventPayload.CompetitorConfig(){
                                CompetitorId= CompetitorIds.ShopA,
                                ProductExternalIds= new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId>
                                {
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId(){ Id="Unique1", Name=ProductExternalIdNames.UniqueId},
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId(){ Id="EAN1", Name=ProductExternalIdNames.EAN}
                                },
                                ProductUrls= new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductUrl>
                                {
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductUrl(){
                                        Name = ProductUrlNames.ProductProfile,
                                        Url = "https://product.com/Id1"
                                    }
                                }
                            }
                        }
                }
            };

            yield return new object[]
            {
                new AddOrEditProductModels.AddOrEditProductRequest()
                {
                    ProductId = "ProductId",
                    ProductName = "ProductName",
                    Price = 100,
                    Quantity = 10,
                    Strategys = new List<AddOrUPdateProductRequestedEventPayload.Strategy>(),
                    CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>
                        {
                            new AddOrUPdateProductRequestedEventPayload.CompetitorConfig(){
                                CompetitorId = CompetitorIds.ShopA,
                                ProductExternalIds = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId>
                                {
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId(){ Id="Unique1", Name=ProductExternalIdNames.UniqueId},
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductExternalId(){ Id="EAN1", Name=ProductExternalIdNames.EAN}
                                },
                                ProductUrls= new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductUrl>
                                {
                                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig.ProductUrl(){
                                        Name = ProductUrlNames.ProductProfile,
                                        Url = "https://product.com/Id1"
                                    }
                                }
                            }
                        }
                }
            };

            yield return new object[]
            {
                new AddOrEditProductModels.AddOrEditProductRequest()
                {
                    ProductId = "ProductId",
                    ProductName = "ProductName",
                    Price = 100,
                    Quantity = 10,
                    Strategys = new List<AddOrUPdateProductRequestedEventPayload.Strategy> {
                            new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallAveragePrice},
                            new AddOrUPdateProductRequestedEventPayload.Strategy() { Id = StrategyIds.OverallCheaperPrice}
                        },
                    CompetitorConfigs = null
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
