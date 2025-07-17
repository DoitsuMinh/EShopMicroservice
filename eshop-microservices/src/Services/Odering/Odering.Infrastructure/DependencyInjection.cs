using FluentMigrator.Runner;
using Odering.Infrastructure.ExternalServices.FreeCurrencyApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Odering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddMarten(opts =>
        {
            opts.Connection(connectionString!);
        }).UseLightweightSessions();

        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString!)
                .ScanIn(typeof(DependencyInjection).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        // Configure FreeCurrencyApi options
        services.Configure<FreeCurrencyApiOptions>(
            configuration.GetSection(FreeCurrencyApiOptions.SectionName));

        // Register HttpClient for FreeCurrencyApi
        services.AddHttpClient<IFreeCurrencyApiService, FreeCurrencyApiService>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<FreeCurrencyApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        });

        // Register other services...
        services.AddScoped<IForeignExchange, ForeignExchange>();

        return services;
    }
}
