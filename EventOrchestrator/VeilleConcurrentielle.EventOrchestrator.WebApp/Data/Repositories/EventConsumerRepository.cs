using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Repositories
{
    public class EventConsumerRepository : RepositoryBase<EventConsumerEntity>, IEventConsumerRepository
    {
        public EventConsumerRepository(EventDbContext dbContext) : base(dbContext)
        {
        }
    }
}
