using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VeilleConcurrentielle.Infrastructure.TestLib
{
    public abstract class WebAppBase<TEntryPoint, TDbContext> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class where TDbContext : DbContext
    {
        private readonly string _environment;
        private readonly string _dbName;
        public WebAppBase(string environment = "Development", string dbName = "Test")
        {
            _environment = environment;
            _dbName = dbName;
        }
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(_environment);
            builder.ConfigureServices(services =>
            {
                services.AddScoped(sp =>
                {
                    return new DbContextOptionsBuilder<TDbContext>()
                                .UseInMemoryDatabase(_dbName)
                                .UseApplicationServiceProvider(sp)
                                .Options;
                });
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<TDbContext>();
                db.Database.EnsureCreated(); 
            });
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

            });
            base.ConfigureWebHost(builder);
        }
    }
}