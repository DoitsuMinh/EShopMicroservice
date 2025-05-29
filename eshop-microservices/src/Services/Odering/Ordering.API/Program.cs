using Odering.Infrastructure;
using Odering.Infrastructure.Extensions;
using Ordering.API;
using Ordering.Application;


// Shoule be using SQL Server/ MySQL with EF core, but i'm using PostgreSQL with Dapper

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();     // make sure console logging is added
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = false;
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
    options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
});

// Add services to the container.
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();

if(app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
    await app.SeedInitialDataAsync(builder.Configuration.GetConnectionString("Database")!);
}


// Confiugre the HTTP request pipeline.


app.Run();