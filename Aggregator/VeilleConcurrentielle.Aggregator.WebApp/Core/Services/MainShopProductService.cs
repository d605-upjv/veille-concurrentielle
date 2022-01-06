using System.Text.RegularExpressions;
using VeilleConcurrentielle.Aggregator.WebApp.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public class MainShopProductService : IMainShopProductService
    {
        private readonly Regex productIdExpr = new Regex(@"(/\d+)");
        private readonly IMainShopWebService _mainShopWebService;
        public MainShopProductService(IMainShopWebService mainShopWebService)
        {
            _mainShopWebService = mainShopWebService;
        }
        public async Task<MainShopProduct?> GetProductAsync(string productUrl)
        {
            string? productId = GetProductId(productUrl);
            if (productId == null)
            {
                return null;
            }
            var product = await _mainShopWebService.GetProductAsync(productId, productUrl);
            return product;
        }

        public string? GetProductId(string productUrl)
        {
            var firstMatch = productIdExpr.Match(productUrl);
            if (firstMatch.Success)
            {
                var productId = firstMatch.Value.Substring(1);
                return productId;
            }
            return null;
        }
    }
}
