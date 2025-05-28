using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;

namespace Odering.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        //migrator.MigrateUp(20250527000000); // Apply migrations
        //try
        //{
        //    migrator.MigrateUp(20250527000000); // Apply migrations
        //}
        //catch (Exception ex)
        //{
        //    // Handle exceptions during migration
        //    Console.WriteLine($"Migration failed: {ex.Message}");
        //    migrator.Rollback(0);
        //}
        await Task.Run(() =>
        {
            //try
            //{
                migrator.MigrateUp(); // Apply migrations
            //}
            //catch (Exception ex)
            //{
            //    // Handle exceptions during migration
            //    Console.WriteLine($"Migration failed: {ex.Message}");
            //    migrator.Rollback(0);
            //}
        });
        await Task.CompletedTask; // Ensure the method is async
    }
}
