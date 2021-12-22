using System.Text.Json.Serialization;
using VeilleConcurrentielle.Aggregator.WebApp.Data;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.EventOrchestrator.Lib.Servers;
using VeilleConcurrentielle.Infrastructure.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var aggregatorDbConnectionString = builder.Configuration.GetConnectionString("AggregatorDb") ?? "Data Source=aggregator.db";
builder.Services.AddSqlite<AggregatorDbContext>(aggregatorDbConnectionString)
                    .AddDatabaseDeveloperPageExceptionFilter();

var eventServiceUrl = builder.Configuration["Services:EventUrl"];
builder.Services.RegisterEventServiceClientDependencies(eventServiceUrl);
builder.Services.RegisterReceivedEventServiceDependencies<AggregatorDbContext>();

builder.Services.AddScoped<ICompetitorRepository, CompetitorRepository>();
builder.Services.AddScoped<IStrategyRepository, StrategyRepository>();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToFile("index.html"); ;

app.Run();

public partial class Program { }