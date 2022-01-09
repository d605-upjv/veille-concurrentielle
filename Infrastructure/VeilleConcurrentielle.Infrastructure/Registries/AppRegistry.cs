using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VeilleConcurrentielle.Infrastructure.Core.Data.Repositories;

namespace VeilleConcurrentielle.Infrastructure.Registries
{
    public static class AppRegistry
    {
        public static void RegisterReceivedEventServiceDependencies<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddScoped<IReceivedEventRepository, ReceivedEventRepository<TDbContext>>();
        }
    }
}
