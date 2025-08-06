using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Odering.Infrastructure.Caching;
using Odering.Infrastructure.Database;
using Odering.Infrastructure.Domain;
using Odering.Infrastructure.Emails;
using Odering.Infrastructure.Logging;
using Odering.Infrastructure.Processing;
using Odering.Infrastructure.Processing.InternalCommands;
using Odering.Infrastructure.Processing.Outbox;
using Odering.Infrastructure.Quartz;
using Odering.Infrastructure.SeedWork;
using Ordering.Application;
using Ordering.Application.Configuration.Emails;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace Odering.Infrastructure;

public class ApplicationStartup
{
    public static IServiceProvider Initialize(
          IServiceCollection services,
          string connectionString,
          ICacheStore cacheStore,
          IEmailSender emailSender,
          EmailsSettings emailsSetting,
          ILogger logger,
          IExecutionContextAccessor executionContextAccessor,
          bool runQuartz = true)
    {
        if (runQuartz)
        {
            StartQuartz(connectionString, emailsSetting, logger, executionContextAccessor);
        }


        // Register the cache store as a singleton service in the DI container
        services.AddSingleton(cacheStore);

        // Create and configure the Autofac service provider
        var serviceProvider = CreateAutofacServiceProvider(
            services,
            connectionString,
            emailSender,
            emailsSetting,
            logger,
            executionContextAccessor);

        // Return the configured service provider
        return serviceProvider;
    }

    private static void StartQuartz(
        string connectionString, 
        EmailsSettings emailsSetting, 
        ILogger logger, 
        IExecutionContextAccessor executionContextAccessor)
    {
        try
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            var container = new ContainerBuilder();

            container.RegisterModule(new LoggingModule(logger));
            container.RegisterModule(new QuartzModule());
            container.RegisterModule(new DataAccessModule(connectionString));
            container.RegisterModule(new EmailModule(emailsSetting));
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ProcessingModule());


            container.RegisterInstance(executionContextAccessor);
            container.Register(c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<OrdersContext>();
                dbContextOptionsBuilder.UseNpgsql(connectionString);

                dbContextOptionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
                return new OrdersContext(dbContextOptionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();

            scheduler.JobFactory = new JobFactory(container.Build());

            scheduler.Start().GetAwaiter().GetResult();

            var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
            var trigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * ? * *")
                    .Build();

            scheduler.ScheduleJob(processOutboxJob, trigger).GetAwaiter().GetResult();

            var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();
            var triggerCommandsProcessing =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/15 * * ? * *")
                    .Build();

            scheduler.ScheduleJob(processInternalCommandsJob, triggerCommandsProcessing).GetAwaiter().GetResult();
        } catch (Exception e)
        {
            logger.Error(e, "Error during application startup");
            throw;
        }
    }

    private static IServiceProvider CreateAutofacServiceProvider(
        IServiceCollection services,
        string connectionString,
        IEmailSender emailSender,
        EmailsSettings emailsSettings,
        ILogger logger,
        IExecutionContextAccessor executionContextAccessor)
    {
        try
        {
            // Create a new Autofac container builder
            var container = new ContainerBuilder();

            // Populate the container with services from the IServiceCollection
            container.Populate(services);

            // Register modules to the container
            container.RegisterModule(new LoggingModule(logger));
            container.RegisterModule(new DataAccessModule(connectionString));
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new DomainModule());

            // Email module registration
            if (emailSender != null)
            {
                container.RegisterModule(new EmailModule(emailSender, emailsSettings));
            }
            else
            {
                container.RegisterModule(new EmailModule(emailsSettings));
            }

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
        } catch (Exception ex)
        {
            logger.Error(ex, "Error during application startup");
            throw;
        }
       
    }
}
