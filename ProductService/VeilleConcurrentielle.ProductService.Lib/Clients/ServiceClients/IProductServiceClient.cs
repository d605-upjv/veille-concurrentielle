using VeilleConcurrentielle.ProductService.Lib.Clients.Models;

namespace VeilleConcurrentielle.ProductService.Lib.Clients.ServiceClients
{
    public interface IProductServiceClient
    {
        Task<GetProductsToScrapClientResponse> GetProductsToScrapAsync();
    }
}