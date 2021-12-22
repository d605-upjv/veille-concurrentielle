using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Repositories
{
    public class EventSubscriberRepository : RepositoryBase<EventSubscriberEntity>, IEventSubscriberRepository
    {
        public EventSubscriberRepository(EventDbContext dbContext) : base(dbContext)
        {
        }
    }
}
