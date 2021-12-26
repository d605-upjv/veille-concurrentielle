using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public interface IProductPriceService
    {
        Task OnPriceIdentifedAsync(string eventId, PriceIdentifiedEventPayload request);
        Task<ProductPrice?> GetMaxPriceAsync(string productId);
        Task<ProductPrice?> GetMinPriceAsync(string productId);
    }
}