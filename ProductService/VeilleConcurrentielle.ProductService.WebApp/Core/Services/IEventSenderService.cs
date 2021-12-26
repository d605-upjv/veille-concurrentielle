using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public interface IEventSenderService
    {
        Task SendProductAddedOrUpdatedEvent(string refererEventId, ProductEntity productEntity, ProductPrice? minPrice, ProductPrice? maxPrice);
    }
}