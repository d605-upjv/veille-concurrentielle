using VeilleConcurrentielle.Infrastructure.Core.Models;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Models
{
    public class GetRecommendationResponse
    {
        public List<ProductRecommendation> Recommendations { get; set; }
        public List<ProductRecommendation> NewRecommendations { get; set; }
    }
}
