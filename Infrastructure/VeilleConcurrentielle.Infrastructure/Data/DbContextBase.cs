using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Core.Data.Entities;

namespace VeilleConcurrentielle.Infrastructure.Data
{
    public abstract class DbContextBase<T> : DbContext where T : DbContext
    {
        public DbContextBase(DbContextOptions<T> options) : base(options) { }

        public DbSet<ReceivedEventEntity> ReceivedEvents => Set<ReceivedEventEntity>();
    }
}
