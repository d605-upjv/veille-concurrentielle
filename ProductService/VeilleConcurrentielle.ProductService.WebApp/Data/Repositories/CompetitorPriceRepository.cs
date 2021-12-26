using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Data;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Repositories
{
    public class CompetitorPriceRepository : RepositoryBase<CompetitorPriceEntity>, ICompetitorPriceRepository
    {
        public CompetitorPriceRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsDifferentFromLastPriceAsync(string productId, string competitorId, double price, int quantity, DateTime createdAt)
        {
            ProductDbContext dbContext = (ProductDbContext)_dbContext;
            var lastPrice = await dbContext.CompetitorPrices.AsNoTracking()
                                                .Where(e => e.ProductId == productId && e.CompetitorId == competitorId)
                                                .OrderByDescending(e => e.CreatedAt)
                                                .FirstOrDefaultAsync();
            if (lastPrice != null)
            {
                return lastPrice.CreatedAt < createdAt 
                    && !(lastPrice.ProductId == productId
                    && lastPrice.CompetitorId == competitorId
                    && lastPrice.Price == price
                    && lastPrice.Quantity == quantity);
            }
            return true;
        }

        public async Task<List<CompetitorPriceEntity>> GetLastPricesAsync(string productId, string competitorId, int priceCount)
        {
            ProductDbContext dbContext = (ProductDbContext)_dbContext;
            var prices = await dbContext.CompetitorPrices.AsNoTracking()
                                            .Where(e => e.ProductId == productId && e.CompetitorId == competitorId)
                                            .OrderByDescending(e => e.CreatedAt)
                                            .Take(priceCount)
                                            .ToListAsync();
            return prices;
        }
    }
}
