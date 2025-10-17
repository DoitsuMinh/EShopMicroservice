using BuildlingBlocks.Messaging.MassTransit;
using HealthChecks.UI.Client;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.FeatureManagement;
using Odering.Infrastructure;
using Ordering.API.Configuration;
using Ordering.API.SeedWork;
using Ordering.Application;
using Ordering.Application.BasketCheckout;
using Ordering.Application.Configuration.Emails;
using Ordering.Application.Configuration.Validation;
using Ordering.Domain.SeedWork;
using Ordering.Infrastructure.Caching;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Serilog.ILogger;


// Reference project: https://github.com/kgrzybek/sample-dotnet-core-cqrs-api
namespace Ordering.API;

public class Startup
{
    private readonly IConfiguration _configuration;

    private const string ConnectionStrings = "ConnectionStrings:Database";

    private static ILogger _logger; // Initialize _logger directly to avoid nullability issues    

    public Startup(IWebHostEnvironment env)
    {
        _logger = ConfigureLogger();
        _logger.Information("Starting app ...");

        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"hosting.{env.EnvironmentName}.json", optional: true)
            .AddUserSecrets<Startup>() // Load user secrets in development  
            .AddEnvironmentVariables() 
            .Build();
    }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        _logger.Information("Configuring services ...");
        try
        {
            services.AddControllers();

            services.AddMemoryCache();

            services.AddHttpClient();

            services.AddSwaggerDocumentation();

            services.AddHealthChecks().AddNpgSql(_configuration[ConnectionStrings]);

            services.AddMessageBroker(_configuration, typeof(BasketCheckoutIntegrationEventHandler).Assembly);

            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
                x.Map<NotFoundException>(ex => new EntityNotFoundProblemDetails(ex));
            });

            services.AddFeatureManagement();

            services.AddHttpContextAccessor();

            var serviceProvider = services.BuildServiceProvider();

            IExecutionContextAccessor executionContextAccessor = new ExecutionContextAccessor(serviceProvider.GetService<IHttpContextAccessor>());

            var children = _configuration.GetSection("Caching").GetChildren();
            var cachingConfiguration = children.ToDictionary(child => child.Key, child => TimeSpan.Parse(child.Value));
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            var featureManager = serviceProvider.GetService<IFeatureManager>();
            var emailSenderEnabled = featureManager.IsEnabledAsync("EmailSender").GetAwaiter().GetResult();
            if (emailSenderEnabled)
            {
                var emailsSettings = _configuration.GetSection("EmailsSettings").Get<EmailsSettings>();
                var emailSender = serviceProvider.GetService<IEmailSender>();

                var result = ApplicationStartup.Initialize(
                    services,
                    connectionString: _configuration[ConnectionStrings] ?? throw new ArgumentNullException(
                        $"Connection string '{ConnectionStrings}' is not configured."
                        ),
                    cacheStore: new MemoryCacheStore(memoryCache, cachingConfiguration),
                    emailSender,
                    emailsSettings,
                    logger: _logger,
                    executionContextAccessor: executionContextAccessor
                    );

                _logger.Information("Services configured successfully.");
                return result;
            }
            else
            {
                var result = ApplicationStartup.Initialize(
                    services,
                    connectionString: _configuration[ConnectionStrings],
                    cacheStore: new MemoryCacheStore(memoryCache, cachingConfiguration),
                    emailSender: null, // Disable email sender
                    null, // Disable email settings
                    logger: _logger,
                    executionContextAccessor: executionContextAccessor
                );
                _logger.Information("Services configured successfully.");
                return result;
            }
        }
        catch (Exception)
        {
            _logger.Error("An error occurred during service configuration.");
            throw;
        }

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        _logger.Information("Configuring application ...");
        
        app.UseMiddleware<CorrelationMiddleware>();
        app.UseProblemDetails();
        
        if (env.IsDevelopment())
        {
            //app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseProblemDetails();
            app.UseHsts();
        }

        // Move health checks BEFORE routing
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseSwaggerDocumentation();

        _logger.Information("Application configured successfully.");
    }

    private static ILogger ConfigureLogger()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Literate,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(new CompactJsonFormatter(), $"logs/logs-{DateTime.UtcNow:yyyy-MM-dd}.txt")
            .CreateLogger();
    }
}
