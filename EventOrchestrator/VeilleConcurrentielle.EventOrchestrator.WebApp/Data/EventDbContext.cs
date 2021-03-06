using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Data
{
    public class EventDbContext : DbContextBase<EventDbContext>
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
        }

        public DbSet<EventEntity> Events => Set<EventEntity>();
        public DbSet<EventSubscriberEntity> EventSubscribers => Set<EventSubscriberEntity>();
        public DbSet<EventConsumerEntity> EventConsumers => Set<EventConsumerEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventSubscriberEntity>().HasData(
                new EventSubscriberEntity() { Id = "7b68fa7b-0062-478e-ac70-111733f6cdee", EventName = EventNames.Test.ToString(), ApplicationName = ApplicationNames.EventOrchestrator.ToString() },
                new EventSubscriberEntity() { Id = "2e778f3b-a580-43e2-b292-774d5a070d92", EventName = EventNames.Test.ToString(), ApplicationName = ApplicationNames.ProductService.ToString() },
                new EventSubscriberEntity() { Id = "a7d97a94-7d70-4415-bbbf-2956d5879b2b", EventName = EventNames.Test.ToString(), ApplicationName = ApplicationNames.Aggregator.ToString() },
                new EventSubscriberEntity() { Id = "212d189d-c176-4490-b7c8-edd0be4b3dff", EventName = EventNames.AddOrUpdateProductRequested.ToString(), ApplicationName = ApplicationNames.ProductService.ToString() },
                new EventSubscriberEntity() { Id = "92e062c7-7372-43e2-957d-47c51e91bc16", EventName = EventNames.ProductAddedOrUpdated.ToString(), ApplicationName = ApplicationNames.Aggregator.ToString() },
                new EventSubscriberEntity() { Id = "3c0e3b84-e3e6-4203-97c3-7f9159fd3bc0", EventName = EventNames.PriceIdentified.ToString(), ApplicationName = ApplicationNames.ProductService.ToString() },
                new EventSubscriberEntity() { Id = "45037b0a-561d-44df-9d14-47bf2f21487d", EventName = EventNames.NewRecommendationPushed.ToString(), ApplicationName = ApplicationNames.Aggregator.ToString() }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
