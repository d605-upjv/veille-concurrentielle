using VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public class RecommendationAlertService : IRecommendationAlertService
    {
        private readonly IRecommendationAlertRepository _recommedationAlertRepository;
        public RecommendationAlertService(IRecommendationAlertRepository recommendationAlertRepository)
        {
            _recommedationAlertRepository = recommendationAlertRepository;
        }

        public async Task StoreNewRecommendationAlertAsync(string refererEventId, NewRecommendationPushedEventPayload request)
        {
            RecommendationAlertEntity alertEntity = new RecommendationAlertEntity()
            {
                ProductId = request.ProductId,
                IsSeen = false,
                CurrentPrice = request.Recommendation.CurrentPrice,
                Price = request.Recommendation.Price,
                StrategyId = request.Recommendation.StrategyId.ToString(),
                CreatedAt = request.Recommendation.CreatedAt,
            };
            await _recommedationAlertRepository.InsertAsync(alertEntity);
        }

        public async Task<SetSeenRecommendationAlertModels.SetSeenRecommendationAlertResponse?> SetToSeenAsync(string recommendationAlertId)
        {
            var existing = await _recommedationAlertRepository.GetByIdAsync(recommendationAlertId);
            if (existing == null)
            {
                return null;
            }
            existing.IsSeen = true;
            existing.SeenAt = DateTime.Now;
            await _recommedationAlertRepository.UpdateAsync(existing);
            return new SetSeenRecommendationAlertModels.SetSeenRecommendationAlertResponse() { SeenAt = existing.SeenAt.Value };
        }

        public async Task<GetAllUnseenRecommendationAlertModels.GetAllUnseenRecommendationAlertResponse> GetAlUnseenAsync()
        {
            var items = (await _recommedationAlertRepository.GetAllUnseenAsync())
                                    .Select(e => new GetAllUnseenRecommendationAlertModels.GetAllUnseenRecommendationAlertResponse.RecommendationAlert()
                                    {
                                        Id = e.Id,
                                        ProductId = e.ProductId,
                                        IsSeen = e.IsSeen,
                                        CreatedAt = e.CreatedAt,
                                        CurrentPrice = e.CurrentPrice,
                                        Price = e.Price,
                                        StrategyId = EnumUtils.GetValueFromString<StrategyIds>(e.StrategyId)
                                    }).ToList();
            return new GetAllUnseenRecommendationAlertModels.GetAllUnseenRecommendationAlertResponse()
            {
                Recommendations = items
            };
        }
    }
}
