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
                new EventSubscriberEntity() { Id = "212d189d-c176-4490-b7c8-edd0be4b3dff", EventName = EventNames.AddOrUpdateProductRequested.ToString(), ApplicationName = ApplicationNames.ProductService.ToString() },
                new EventSubscriberEntity() { Id = "7b68fa7b-0062-478e-ac70-111733f6cdee", EventName = EventNames.Test.ToString(), ApplicationName = ApplicationNames.EventOrchestrator.ToString() },
                new EventSubscriberEntity() { Id = "2e778f3b-a580-43e2-b292-774d5a070d92", EventName = EventNames.Test.ToString(), ApplicationName = ApplicationNames.ProductService.ToString() },
                new EventSubscriberEntity() { Id = "a7d97a94-7d70-4415-bbbf-2956d5879b2b", EventName = EventNames.Test.ToString(), ApplicationName = ApplicationNames.Aggregator.ToString() }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
