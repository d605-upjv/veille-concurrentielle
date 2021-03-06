using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Repositories
{
    public interface IEventSubscriberRepository : IRepository<EventSubscriberEntity>
    {
    }
}
