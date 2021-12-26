using VeilleConcurrentielle.Infrastructure.Data;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Repositories
{
    public interface ICompetitorPriceRepository : IRepository<CompetitorPriceEntity>
    {
        Task<bool> IsDifferntFromLastPriceAsync(string productId, string competitorId, double price, int quantity);
        Task<CompetitorPriceEntity?> GetMinPriceAsync(string productId);
        Task<CompetitorPriceEntity?> GetMaxPriceAsync(string productId);
    }
}
