using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public interface IRecommendationAlertService
    {
        Task StoreNewRecommendationAlertAsync(string refererEventId, NewRecommendationPushedEventPayload request);
        Task<GetAllUnseenRecommendationAlertModels.GetAllUnseenRecommendationAlertResponse> GetAlUnseenAsync();
        Task<SetRecommendationAlertToSeenModels.SetRecommendationAlertToSeenResponse?> SetToSeenAsync(string recommendationAlertId);
        Task<GetAllUnseenRecommendationAlertCountModels.GetAllUnseenRecommendationAlertCountResponse> GetAllUnseenCountAsync();
        Task<SetRecommendationAlertForProductToSeenModels.SetRecommendationAlertForProductToSeenResponse> SetToSeenForProductAsync(string productId);
        Task<GetAllUnseenRecommendationAlertCountByProductModels.GetAllUnseenRecommendationAlertCountByProductResponse> GetAllUnseenCountByProductAsync();
    }
}