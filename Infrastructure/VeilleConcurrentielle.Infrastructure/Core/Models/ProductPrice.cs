﻿namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public class ProductPrice
    {
        public double Price { get; set; }
        public int Quantity { get; set; }
        public CompetitorIds CompetitorId { get; set; }
    }
}
