using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.ProductService.WebApp.Data
{
    public class ProductDbContext : DbContextBase<ProductDbContext>
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }
    }
}
