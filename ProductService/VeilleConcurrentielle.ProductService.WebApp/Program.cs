using System.Text.Json.Serialization;
using VeilleConcurrentielle.EventOrchestrator.Lib.Registries;
using VeilleConcurrentielle.Infrastructure.Registries;
using VeilleConcurrentielle.ProductService.WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var productDbConnectionString = builder.Configuration.GetConnectionString("ProductDb") ?? "Data Source=product.db";
builder.Services.AddSqlite<ProductDbContext>(productDbConnectionString)
                    .AddDatabaseDeveloperPageExceptionFilter();

builder.Services.RegisterEventServiceClientDependencies(builder.Configuration);
builder.Services.RegisterReceivedEventServiceDependencies<ProductDbContext>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }