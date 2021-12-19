using Microsoft.EntityFrameworkCore;

namespace VeilleConcurrentielle.Infrastructure.Data
{
    public abstract class DbContextBase<T> : DbContext where T : DbContext
    {
        public DbContextBase(DbContextOptions<T> options) : base(options) { }
    }
}
