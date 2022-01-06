using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VeilleConcurrentielle.Infrastructure.Core.Configurations;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.ServiceClients;
using VeilleConcurrentielle.ProductService.Lib.Clients.Models;
using VeilleConcurrentielle.ProductService.Lib.Servers.Models;

namespace VeilleConcurrentielle.ProductService.Lib.Clients.ServiceClients
{
    public class ProductServiceClient : ServiceClientBase, IProductServiceClient
    {
        public ProductServiceClient(HttpClient httpClient, IOptions<ServiceUrlsOptions> serviceUrlOptions, ILogger<ProductServiceClient> logger)
            : base(httpClient, serviceUrlOptions, logger)
        {
        }

        protected override string Controller => "Products";

        public async Task<GetProductsToScrapClientResponse> GetProductsToScrapAsync()
        {
            var serverResponse = await GetAsync<GetProductsToScrapServerResponse>(GetServiceUrl(ApplicationNames.ProductService), "scrap");
            return new GetProductsToScrapClientResponse()
            {
                Products = serverResponse.Products
            };
        }
    }
}
