using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.ProductService.Lib.Servers.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public interface IProductsService
    {
        Task OnAddOrUPdateProductRequestedAsync(string refererEventId, AddOrUPdateProductRequestedEventPayload request);
        Task<List<ProductToScrap>> GetProductsToScrap();
    }
}