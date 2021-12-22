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
                new EventSubscriberEntity() { Id = "212d189d-c176-4490-b7c8-edd0be4b3dff", EventName = EventNames.AddOrUpdateProductRequested.ToString(), ApplicationName = ApplicationNames.ProductService.ToString() }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
