namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class GetAllUnseenRecommendationAlertCountByProductModels
    {
        public class GetAllUnseenRecommendationAlertCountByProductResponse
        {
            public List<RecommendationAlertCountByProduct> Items { get; set; }
        }

        public class RecommendationAlertCountByProduct
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public int Count { get; set; }
        }
    }
}
