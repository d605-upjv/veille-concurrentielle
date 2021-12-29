// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VeilleConcurrentielle.EventOrchestrator.Lib.Registries;
using VeilleConcurrentielle.ProductService.Lib.Registries;
using VeilleConcurrentielle.Scraper.ConsoleApp;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((context, services) =>
{
    services.RegisterEventServiceClientDependencies(context.Configuration);
    services.RegisterProductServiceClientDependencies();
    services.Configure<WorkerConfigOptions>(context.Configuration.GetSection(WorkerConfigOptions.WorkerConfig));
    services.AddScoped<IWebScraperWorker, WebScraperWorker>();
    services.AddScoped<IAppTerminator, AppTerminator>();
    services.AddScoped<IPriceSearcher, PriceSearcher>();
    services.AddScoped<IHtmlDocumentLoader, HtmlDocumentLoader>();
});
var app = builder.Build();

var worker = app.Services.GetRequiredService<IWebScraperWorker>();
await worker.RunAsync();