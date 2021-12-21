using VeilleConcurrentielle.Aggregator.WebApp.Data;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var eventDbConnectionString = builder.Configuration.GetConnectionString("AggregatorDb") ?? "Data Source=aggregator.db";
builder.Services.AddSqlite<AggregatorDbContext>(eventDbConnectionString)
                    .AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<ICompetitorRepository, CompetitorRepository>();
builder.Services.AddScoped<IStrategyRepository, StrategyRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();

public partial class Program { }