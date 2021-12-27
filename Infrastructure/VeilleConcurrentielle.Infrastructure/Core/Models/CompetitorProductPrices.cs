﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public class CompetitorProductPrices
    {
        [Required]
        public List<CompetitorItemProductPrices> Prices { get; set; }
        public ProductMinMaxPrice? MinPrice { get; set; }
        public ProductMinMaxPrice? MaxPrice { get; set; }

        public List<ProductPrice> GetLatestPricesPerCompetitor()
        {
            if (Prices != null)
            {
                return Prices.Select(e => e.Prices).Select(e => e.First()).ToList();
            }
            return new List<ProductPrice>();
        }

        public class CompetitorItemProductPrices
        {
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public CompetitorIds CompetitorId { get; set; }
            [Required]
            public List<ProductPrice> Prices { get; set; }
        }

        public class ProductMinMaxPrice : ProductPrice
        {
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public CompetitorIds CompetitorId { get; set; }
        }
    }
}
