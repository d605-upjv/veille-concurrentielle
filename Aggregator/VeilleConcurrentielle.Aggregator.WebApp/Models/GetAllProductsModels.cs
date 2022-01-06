using VeilleConcurrentielle.Aggregator.WebApp.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class GetAllProductsModels
    {
        public class GetAllProductsResponse
        {
            public List<Product> Products { get; set; }
        }
    }
}
