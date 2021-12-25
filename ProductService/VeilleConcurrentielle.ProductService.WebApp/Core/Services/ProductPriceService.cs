using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public class ProductPriceService : IProductPriceService
    {
        public async Task<ProductPrice?> GetMinPriceAsync(string productId)
        {
            return null;
        }

        public async Task<ProductPrice?> GetMaxPriceAsync(string productId)
        {
            return null;
        }
    }
}
