using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data
{
    public class AggregatorDbContext : DbContextBase<AggregatorDbContext>
    {
        public AggregatorDbContext(DbContextOptions<AggregatorDbContext> options) : base(options)
        {
        }

        public DbSet<CompetitorEntity> Competitors => Set<CompetitorEntity>();
        public DbSet<StrategyEntity> Strategies => Set<StrategyEntity>();
        public DbSet<ProductAggregateEntity> ProductAggregates => Set<ProductAggregateEntity>();
        public DbSet<ProductAggregateStrategyEntity> ProductAggregateStrategies => Set<ProductAggregateStrategyEntity>();
        public DbSet<ProductAggregateCompetitorConfigEntity> ProductAggregateCompetitorConfigs => Set<ProductAggregateCompetitorConfigEntity>();
        public DbSet<ProductAggregatePriceEntity> ProductAggregatePrices => Set<ProductAggregatePriceEntity>();
        public DbSet<ProductAggregateRecommendationEntity> ProductRecommendations => Set<ProductAggregateRecommendationEntity>();
        public DbSet<RecommendationAlertEntity> RecommendationAlerts => Set<RecommendationAlertEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupModelRelations(modelBuilder);
            SeedData(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SetupModelRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductAggregateEntity>()
                            .HasMany(e => e.Strategies)
                            .WithOne(e => e.Product)
                            .OnDelete(DeleteBehavior.Cascade)
                            .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<ProductAggregateEntity>()
                            .HasMany(e => e.CompetitorConfigs)
                            .WithOne(e => e.Product)
                            .OnDelete(DeleteBehavior.Cascade)
                            .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<ProductAggregateEntity>()
                            .HasMany(e => e.LastPrices)
                            .WithOne(e => e.Product)
                            .OnDelete(DeleteBehavior.Cascade)
                            .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<ProductAggregateEntity>()
                            .HasMany(e => e.Recommendations)
                            .WithOne(e => e.Product)
                            .OnDelete(DeleteBehavior.Cascade)
                            .HasForeignKey(e => e.ProductId);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompetitorEntity>().HasData(
                new CompetitorEntity() { Id = CompetitorIds.ShopA.ToString(), Name = "Shop A", LogoUrl = "https://www.icone-png.com/png/43/43296.png" },
                new CompetitorEntity() { Id = CompetitorIds.ShopB.ToString(), Name = "Shop A", LogoUrl = "https://www.icone-png.com/png/43/43302.png" }
                );

            modelBuilder.Entity<StrategyEntity>().HasData(
                new StrategyEntity() { Id = StrategyIds.OverallAveragePrice.ToString(), Name = "Dans la moyenne" },
                new StrategyEntity() { Id = StrategyIds.OverallCheaperPrice.ToString(), Name = "Le moins cher" },
                new StrategyEntity() { Id = StrategyIds.FivePercentAboveMeanPrice.ToString(), Name = "5% plus cher que la moyenne" }
                );
        }
    }
}
