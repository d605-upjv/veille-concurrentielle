using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Data
{
    public class EventDbContext : DbContextBase<EventDbContext>
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
        }

        public DbSet<EventEntity> Events => Set<EventEntity>();
    }
}
