using Autofac.Extensions.DependencyInjection;
using Autofac;
using CommonServiceLocator;
using Odering.Infrastructure.Caching;
using Odering.Infrastructure.Database;
using Odering.Infrastructure.Logging;
using Ordering.Application;
using Serilog;
using Autofac.Extras.CommonServiceLocator;
using Odering.Infrastructure.Processing;

namespace Odering.Infrastructure;

public class ApplicationStartup
{
    public static IServiceProvider Initialize(
          IServiceCollection services,
          string connectionString,
          ICacheStore cacheStore,
          ILogger logger,
          IExecutionContextAccessor executionContextAccessor)
    {
        // Register the cache store as a singleton service in the DI container
        services.AddSingleton(cacheStore);

        // Create and configure the Autofac service provider
        var serviceProvider = CreateAutofacServiceProvider(
            services,
            connectionString,
            logger,
            executionContextAccessor);

        // Return the configured service provider
        return serviceProvider;
    }

    private static IServiceProvider CreateAutofacServiceProvider(
       IServiceCollection services,
       string connectionString,
       ILogger logger,
       IExecutionContextAccessor executionContextAccessor)
    {
        // Create a new Autofac container builder
        var container = new ContainerBuilder();

        // Populate the container with services from the IServiceCollection
        container.Populate(services);

        // Register modules to the container
        container.RegisterModule(new LoggingModule(logger));
        container.RegisterModule(new DataAccessModule(connectionString));
        container.RegisterModule(new ProcessingModule());

        // Register the execution context accessor as a singleton instance
        container.RegisterInstance(executionContextAccessor);

        // Build the Autofac container
        var buildContainer = container.Build();

        // Set the service locator provider to use Autofac
        ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(buildContainer));

        // Create an Autofac service provider from the built container
        var serviceProvider = new AutofacServiceProvider(buildContainer);

        // Set the built container as the composition root for the application
        CompositionRoot.SetContainer(buildContainer);

        // Return the configured service provider
        return serviceProvider;
    }
}
