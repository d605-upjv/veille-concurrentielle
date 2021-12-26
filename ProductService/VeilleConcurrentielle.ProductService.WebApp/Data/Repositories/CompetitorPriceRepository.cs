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

        public async Task<bool> IsDifferntFromLastPriceAsync(string productId, string competitorId, double price, int quantity)
        {
            ProductDbContext dbContext = (ProductDbContext)_dbContext;
            var lastPrice = await dbContext.CompetitorPrices.AsNoTracking()
                                                .Where(e => e.ProductId == productId && e.CompetitorId == competitorId)
                                                .OrderByDescending(e => e.CreatedAt)
                                                .LastOrDefaultAsync();
            if (lastPrice != null)
            {
                return !(lastPrice.ProductId == productId
                    && lastPrice.CompetitorId == competitorId
                    && lastPrice.Price == price
                    && lastPrice.Quantity == quantity);
            }
            return true;
        }

        public async Task<CompetitorPriceEntity?> GetMinPriceAsync(string productId)
        {
            ProductDbContext dbContext = (ProductDbContext)_dbContext;
            var price = await dbContext.CompetitorPrices.AsNoTracking()
                                    .Where(e => e.ProductId == productId)
                                    .OrderBy(e => e.Price)
                                    .FirstOrDefaultAsync();
            return price;
        }

        public async Task<CompetitorPriceEntity?> GetMaxPriceAsync(string productId)
        {
            ProductDbContext dbContext = (ProductDbContext)_dbContext;
            var price = await dbContext.CompetitorPrices.AsNoTracking()
                                    .Where(e => e.ProductId == productId)
                                    .OrderByDescending(e => e.Price)
                                    .FirstOrDefaultAsync();
            return price;
        }
    }
}
