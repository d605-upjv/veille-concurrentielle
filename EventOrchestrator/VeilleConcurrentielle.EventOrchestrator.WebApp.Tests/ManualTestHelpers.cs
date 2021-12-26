using System;
using System.Collections.Generic;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using Xunit;
using Xunit.Abstractions;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Tests
{
    [Trait("__NotUnitTests__", "__Manual Test Helpers__")]
    public class ManualTestHelpers
    {
        private readonly ITestOutputHelper _output;
        public ManualTestHelpers(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GenerateSerialized_TestEventPayload()
        {
            TestEventPayload payload = new TestEventPayload()
            {
                IntData = 5,
                StringData = "String",
                RefererEventId = "EventId"
            };
            _output.WriteLine(SerializationUtils.Serialize(payload));
        }

        [Fact]
        public void GenerateSerialized_AddOrUPdateProductRequestedEventPayload()
        {
            AddOrUPdateProductRequestedEventPayload payload = new AddOrUPdateProductRequestedEventPayload()
            {
                ProductId = "ProductId",
                Strategies = new List<AddOrUPdateProductRequestedEventPayload.Strategy>
                {
                    new AddOrUPdateProductRequestedEventPayload.Strategy()
                },
                CompetitorConfigs = new List<AddOrUPdateProductRequestedEventPayload.CompetitorConfig>
                {
                    new AddOrUPdateProductRequestedEventPayload.CompetitorConfig()
                    {
                        Holder = new ConfigHolder()
                        {
                            Items = new List<ConfigHolder.ConfigItem>
                            {
                                new ConfigHolder.ConfigItem()
                            }
                        }
                    }
                }
            };
            _output.WriteLine(SerializationUtils.Serialize(payload));
        }

        [Fact]
        public void GenerateSerialized_PriceIdentifiedEventPayload()
        {
            PriceIdentifiedEventPayload payload = new PriceIdentifiedEventPayload()
            {
            };
            _output.WriteLine(SerializationUtils.Serialize(payload));
        }

        [Fact]
        public void GenerateSerialized_PushEventServerRequest_PriceIdentified()
        {
            PriceIdentifiedEventPayload payload = new PriceIdentifiedEventPayload()
            {
                ProductId = "Productid",
                CompetitorId = CompetitorIds.ShopA,
                Price = 100,
                Quantity = 10,
                Source = PriceSources.PriceApi,
                CreatedAt = DateTime.Now
            };
            var serializedPayload = SerializationUtils.Serialize(payload);
            PushEventServerRequest request = new PushEventServerRequest();
            request.EventName = EventNames.PriceIdentified;
            request.Source = EventSources.Test;
            request.SerializedPayload = serializedPayload;
            _output.WriteLine(SerializationUtils.Serialize(request));
        }
    }
}
