using VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories
{
    public class CompetitorRepository : RepositoryBase<CompetitorEntity>, ICompetitorRepository
    {
        public CompetitorRepository(AggregatorDbContext dbContext) : base(dbContext)
        {
        }

        public override void ComputeNewIdBeforeInsert(CompetitorEntity entity)
        {
            // Id is not auto-generated
        }
    }
}
