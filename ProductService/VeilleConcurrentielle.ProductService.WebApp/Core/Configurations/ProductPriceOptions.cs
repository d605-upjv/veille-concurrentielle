namespace VeilleConcurrentielle.ProductService.WebApp.Core.Configurations
{
    public class ProductPriceOptions
    {
        public const string ProductPrice = "ProductPrice";
        public int HistoryPriceCount { get; set; } = 10;
    }
}
