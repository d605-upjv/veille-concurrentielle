using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Core.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Infrastructure.Core.Data.Repositories
{
    public class ReceivedEventRepository<TDbContext> : RepositoryBase<ReceivedEventEntity>, IReceivedEventRepository
        where TDbContext: DbContext
    {
        public ReceivedEventRepository(TDbContext dbContext) : base(dbContext)
        {
        }
    }
}
