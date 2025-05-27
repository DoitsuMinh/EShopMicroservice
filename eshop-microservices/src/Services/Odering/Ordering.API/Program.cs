using FluentMigrator.Runner;
using Odering.Infrastructure;
using Ordering.API;
using Ordering.Application;


// Shoule be using SQL Server/ MySQL with EF core, but i'm using PostgreSQL with Dapper

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();


var app = builder.Build();

using var scope = app.Services.CreateScope();
var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
migrator.MigrateUp(); // Apply migrations

// Confiugre the HTTP request pipeline.


app.Run();
