using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public interface IEventSenderService
    {
        Task SendProductAddedOrUpdatedEvent(string refererEventId, ProductEntity productEntity, CompetitorProductPrices lastCompetitorPrices, List<ProductRecommendation> recommendations);
        Task SendNewRecommendationPushedEvent(string refererEventId, string productId, List<ProductRecommendation> newRecommendations);
    }
}