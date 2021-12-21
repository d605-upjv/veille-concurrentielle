using System.Text.Json.Serialization;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Core.Services;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
var eventDbConnectionString = builder.Configuration.GetConnectionString("EventDb") ?? "Data Source=events.db";
builder.Services.AddSqlite<EventDbContext>(eventDbConnectionString)
                    .AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();

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