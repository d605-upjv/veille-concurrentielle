using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Repositories
{
    public class EventRepository : RepositoryBase<EventEntity>, IEventRepository
    {
        public EventRepository(EventDbContext dbContext) : base(dbContext)
        {
        }

        public EventEntity GetNextEvent()
        {
            return _dbContext.Set<EventEntity>()
                        .Where(e => e.IsConsumed == false)
                        .OrderBy(e => e.CreatedAt)
                        .FirstOrDefault();
        }
    }
}
