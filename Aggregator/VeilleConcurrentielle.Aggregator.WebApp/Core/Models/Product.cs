namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Models
{
    public class Product
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public double? MinPrice { get; set; }
        public string? MinPriceCompetitorId { get; set; }
        public string? MinPriceCompetitorName { get; set; }
        public string? MinPriceCompetitorLogoUrl { get; set; }
        public int? MinPriceQuantity { get; set; }
        public double? MaxPrice { get; set; }
        public string? MaxPriceCompetitorId { get; set; }
        public string? MaxPriceCompetitorName { get; set; }
        public string? MaxPriceCompetitorLogoUrl { get; set; }
        public int? MaxPriceQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ShopProductId { get; set; }
        public string ShopProductUrl { get; set; }

        public List<ProductStrategy> Strategies { get; set; }
        public List<ProductCompetitorConfig> CompetitorConfigs { get; set; }
        public List<ProductCompetitorPrice> LastPrices { get; set; }
        public List<ProductCompetitorPrice> LastPricesOfAlCompetitors { get; set; }
        public List<ProductRecommendation> Recommendations { get; set; }
    }
}
