using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.ProductService.WebApp.Core.Models;
using VeilleConcurrentielle.ProductService.WebApp.Core.Services.Recommendations;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly Dictionary<StrategyIds, RecommendationEngine> _engines;
        private readonly IRecommendationRepository _recommendationRepository;
        public RecommendationService(IRecommendationRepository recommendationRepository)
        {
            _recommendationRepository = recommendationRepository;
            _engines = new Dictionary<StrategyIds, RecommendationEngine>()
            {
                { StrategyIds.OverallAveragePrice, new OverallAveragePriceEngine() },
                { StrategyIds.OverallCheaperPrice, new OverallCheaperPriceEngine() },
                { StrategyIds.FivePercentAboveMeanPrice, new FivePercentAboveMeanPriceEngine() }
            };
        }
        public RecommendationEngine GetRecommendationEngine(StrategyIds strategyId)
        {
            return _engines[strategyId];
        }
        public async Task<GetRecommendationResponse> GetRecommendationsAsync(GetRecommendationRequest request)
        {
            GetRecommendationResponse response = new GetRecommendationResponse()
            {
                Recommendations = new List<ProductRecommendation>(),
                NewRecommendations = new List<ProductRecommendation>()
            };
            foreach (var strategyId in request.Strategies)
            {
                var latestRecommendation = await _recommendationRepository.GetLatestRecommendationAsync(request.ProductId, strategyId.ToString());
                var strategyEngine = GetRecommendationEngine(strategyId);
                var price = strategyEngine.GetRecommendedPrice(request.Price, request.Quantity, request.LastCompetitorPrices);
                if (price != request.Price)
                {
                    if (latestRecommendation == null || latestRecommendation.Price != price)
                    {
                        latestRecommendation = latestRecommendation ?? new RecommendationEntity()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CurrentPrice = request.Price,
                            Price = price,
                            CreatedAt = DateTime.Now,
                            ProductId = request.ProductId,
                            StrategyId = strategyId.ToString()
                        };
                        response.NewRecommendations.Add(new ProductRecommendation()
                        {
                            Id = latestRecommendation.Id,
                            CurrentPrice = request.Price,
                            Price = latestRecommendation.Price,
                            CreatedAt = latestRecommendation.CreatedAt,
                            StrategyId = strategyId
                        });
                    }
                    response.Recommendations.Add(new ProductRecommendation()
                    {
                        Id = latestRecommendation.Id,
                        CurrentPrice = request.Price,
                        Price = latestRecommendation.Price,
                        CreatedAt = latestRecommendation.CreatedAt,
                        StrategyId = strategyId
                    });
                }
            }
            return response;
        }
    }
}
