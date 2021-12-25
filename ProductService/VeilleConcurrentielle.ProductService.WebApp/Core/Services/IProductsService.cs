using VeilleConcurrentielle.Infrastructure.Core.Models.Events;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public interface IProductsService
    {
        Task StoreProductAsync(string refererEventId, AddOrUPdateProductRequestedEventPayload request);
    }
}