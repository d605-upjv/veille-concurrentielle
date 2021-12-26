using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Data;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;

namespace VeilleConcurrentielle.ProductService.WebApp.Data
{
    public class ProductDbContext : DbContextBase<ProductDbContext>
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<StrategyEntity> Strategies => Set<StrategyEntity>();
        public DbSet<CompetitorConfigEntity> CompetitorConfigs => Set<CompetitorConfigEntity>();
        public DbSet<CompetitorPriceEntity> CompetitorPrices => Set<CompetitorPriceEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>()
                            .HasMany(e => e.Strategies)
                            .WithOne(e => e.Product)
                            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductEntity>()
                            .HasMany(e => e.CompetitorConfigs)
                            .WithOne(e => e.Product)
                            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductEntity>()
                            .HasMany(e => e.CompetitorPrices)
                            .WithOne(e => e.Product)
                            .OnDelete(DeleteBehavior.SetNull);
            base.OnModelCreating(modelBuilder);
        }
    }
}
