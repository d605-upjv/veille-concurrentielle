using Microsoft.AspNetCore.HttpOverrides;
using System.Text.Json.Serialization;
using VeilleConcurrentielle.Aggregator.WebApp.Core.Services;
using VeilleConcurrentielle.Aggregator.WebApp.Data;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using VeilleConcurrentielle.EventOrchestrator.Lib.Registries;
using VeilleConcurrentielle.Infrastructure.Core.Services;
using VeilleConcurrentielle.Infrastructure.Registries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var aggregatorDbConnectionString = builder.Configuration.GetConnectionString("AggregatorDb") ?? "Data Source=aggregator.db";
builder.Services.AddSqlite<AggregatorDbContext>(aggregatorDbConnectionString)
                    .AddDatabaseDeveloperPageExceptionFilter();

builder.Services.RegisterEventServiceClientDependencies(builder.Configuration);
builder.Services.RegisterReceivedEventServiceDependencies<AggregatorDbContext>();

builder.Services.AddScoped<ICompetitorRepository, CompetitorRepository>();
builder.Services.AddScoped<IStrategyRepository, StrategyRepository>();
builder.Services.AddScoped<IEventProcessor, AggregatorEventProcessor>();
builder.Services.AddScoped<IProductAggregateRepository, ProductAggregateRepository>();
builder.Services.AddScoped<IProductAggregateService, ProductAggregateService>();
builder.Services.AddScoped<IRecommendationAlertRepository, RecommendationAlertRepository>();
builder.Services.AddScoped<IRecommendationAlertService, RecommendationAlertService>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseForwardedHeaders();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToFile("index.html"); ;

app.Run();

public partial class Program { }