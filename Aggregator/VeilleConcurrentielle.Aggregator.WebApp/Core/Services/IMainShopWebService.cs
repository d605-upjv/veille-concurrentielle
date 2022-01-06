using VeilleConcurrentielle.Aggregator.WebApp.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public interface IMainShopWebService
    {
        string CleanCDataForUpdate(string content);
        Task<MainShopProduct?> GetProductAsync(string productId, string productUrl);
        Task<string?> UpdateProductPriceAsync(string productId, double newPrice);
    }
}