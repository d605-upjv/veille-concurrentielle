using VeilleConcurrentielle.Aggregator.WebApp.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public interface IMainShopProductService
    {
        Task<MainShopProduct?> GetProductAsync(string productUrl);
        string? GetProductId(string productUrl);
    }
}