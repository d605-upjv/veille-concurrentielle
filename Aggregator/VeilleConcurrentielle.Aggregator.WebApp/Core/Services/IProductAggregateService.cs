using VeilleConcurrentielle.Aggregator.WebApp.Core.Models;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public interface IProductAggregateService
    {
        Task StoreProductAsync(string refererEventId, ProductAddedOrUpdatedEventPayload request);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductbyIdAsync(string productId);
        Task<GetProductToAddOrEditModels.GetProductToAddResponse> GetProductToAddAsync();
        Task<GetProductToAddOrEditModels.GetProductToEditResponse?> GetProductToEditAsync(string productId);
    }
}