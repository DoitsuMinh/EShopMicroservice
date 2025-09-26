using Autofac;
using MediatR;
using Odering.Infrastructure.Logging;
using Odering.Infrastructure.Processing.InternalCommands;
using Ordering.Application.Configuration.Commands;
using Ordering.Application.Configuration.DomainEvents;
using Ordering.Application.Payments;
using System.Reflection;

namespace Odering.Infrastructure.Processing;

public class ProcessingModule : Autofac.Module
{
    /// <summary>
    /// Load the processing module into the Autofac container.
    /// </summary>
    /// <param name="builder"></param>
    protected override void Load(ContainerBuilder builder)
    {

        builder.RegisterType<DomainEventsDispatcher>()
            .As<IDomainEventsDispatcher>()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(PaymentCreatedNotification).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IDomainEventNotification<>)).InstancePerDependency();

        builder.RegisterGenericDecorator(
            typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
            typeof(INotificationHandler<>));

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerDecorator<>),
            typeof(IRequestHandler<>));     // depends on autofac version, older versions may used ICommandHandler<>

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
            typeof(IRequestHandler<,>)      // depends on autofac version, older versions may used ICommandHandler<,>
            );

        builder.RegisterType<CommandsDispatcher>()
            .As<ICommandsDispatcher>()
            .InstancePerLifetimeScope();

        builder.RegisterType<CommandsScheduler>()
            .As<ICommandScheduler>()
            .InstancePerLifetimeScope();

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerDecorator<>),
             typeof(IRequestHandler<>));    // depends on autofac version, older versions may used ICommandHandler<>

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerWithResultDecorator<,>),
            typeof(IRequestHandler<,>));    // depends on autofac version, older versions may used ICommandHandler<,>
    }
}