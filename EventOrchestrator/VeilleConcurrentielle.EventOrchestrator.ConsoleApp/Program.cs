// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VeilleConcurrentielle.EventOrchestrator.ConsoleApp;
using VeilleConcurrentielle.EventOrchestrator.Lib.Registries;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((context, services) =>
{
    services.RegisterEventServiceClientDependencies(context.Configuration);
    services.Configure<WorkerConfigOptions>(context.Configuration.GetSection(WorkerConfigOptions.WorkerConfig));
    services.AddScoped<IEventDispatchWorker, EventDispatchWorker>();
    services.AddScoped<IAppTerminator, AppTerminator>();
});
var app = builder.Build();

var worker = app.Services.GetRequiredService<IEventDispatchWorker>();
await worker.Run();
