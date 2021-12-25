using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public interface IProductPriceService
    {
        Task<ProductPrice?> GetMaxPriceAsync(string productId);
        Task<ProductPrice?> GetMinPriceAsync(string productId);
    }
}