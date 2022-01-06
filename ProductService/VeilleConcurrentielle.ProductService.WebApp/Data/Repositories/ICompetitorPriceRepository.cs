using VeilleConcurrentielle.Infrastructure.Data;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Repositories
{
    public interface ICompetitorPriceRepository : IRepository<CompetitorPriceEntity>
    {
        Task<bool> IsDifferentFromLastPriceAsync(string productId, string competitorId, double price, int quantity, DateTime createdAt);
        Task<List<CompetitorPriceEntity>> GetLastPricesAsync(string productId, string competitorId, int priceCount);
    }
}
