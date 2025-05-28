using Odering.Infrastructure;
using Odering.Infrastructure.Extensions;
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

// Configure the HTTP request pipeline.
app.UseApiServices();

if(app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}


// Confiugre the HTTP request pipeline.


app.Run();
// Use IServiceProvider instead to create a scope.  
//using (var serviceProvider = services.BuildServiceProvider())
//using (var scope = serviceProvider.CreateScope())
//{
//    var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

//    try
//    {
//        migrator.MigrateUp(20250527000000); // Apply migrations  
//    }
//    catch (Exception ex)
//    {
//        // Handle exceptions during migration
//        Console.WriteLine($"Migration failed: {ex.Message}");
//        migrator.Rollback(0);
//    }
//}