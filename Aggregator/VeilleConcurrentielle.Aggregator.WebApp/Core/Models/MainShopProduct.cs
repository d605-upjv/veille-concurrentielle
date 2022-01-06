namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Models
{
    public class MainShopProduct
    {
        public string ShopProductId { get; set; }
        public string ShopProductUrl { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
    }
}
