using Hellang.Middleware.ProblemDetails;
using Odering.Infrastructure;
using Ordering.API.Configuration;
using Ordering.API.SeedWork;
using Ordering.Application;
using Ordering.Application.Configuration.Validation;
using Ordering.Domain.SeedWork;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;


// Reference project: https://github.com/kgrzybek/sample-dotnet-core-cqrs-api
namespace Ordering.API;

public class Startup
{
    private readonly IConfiguration _configuration;

    private const string OrderConnectionString = "OrderingConnectionString";

    private static ILogger _logger = ConfigureLogger(); // Initialize _logger directly to avoid nullability issues    

    public Startup(IWebHostEnvironment env)
    {
        _logger.Information("Logger configured");

        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
            .AddJsonFile($"hosting.{env.EnvironmentName}.json")
            .AddUserSecrets<Startup>() // Load user secrets in development  
            .Build();
    }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddMemoryCache();

        services.AddApplicationServices()
            .AddInfrastructureServices(_configuration)
            .AddApiServices();

        services.AddProblemDetails(x =>
        {
            x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
            x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
        });

        services.AddHttpContextAccessor();
        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<CorrelationMiddleware>();

        if(env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        } 
        else
        {
            app.UseProblemDetails();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private static ILogger ConfigureLogger()
    {
        return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
            .CreateLogger();
    }
}
