using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VeilleConcurrentielle.Infrastructure.Core.Data.Repositories;

namespace VeilleConcurrentielle.Infrastructure.Web
{
    public static class WebAppRegistry
    {
        public static void RegisterReceivedEventServiceDependencies<TDbContext>(this IServiceCollection services) where TDbContext: DbContext
        {
            services.AddScoped<IReceivedEventRepository, ReceivedEventRepository<TDbContext>>();
        }
    }
}
