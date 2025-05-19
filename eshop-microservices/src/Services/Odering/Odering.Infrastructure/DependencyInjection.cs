using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Odering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        //services.AddMarten(opts =>
        //{
        //    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
        //}).UseLightweightSessions();

        //services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
