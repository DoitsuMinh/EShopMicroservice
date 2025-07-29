using Autofac;
using MediatR;
using Odering.Infrastructure.Logging;
using Odering.Infrastructure.Processing.InternalCommands;
using Ordering.Application.Configuration.Commands;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Application.Configuration.DomainEvents;
using Ordering.Application.Payments;
using Ordering.Application.Payments.SendEmailAfterPayment;
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
            typeof(ICommandHandler<>));

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));

        builder.RegisterType<CommandsDispatcher>()
            .As<ICommandsDispatcher>()
            .InstancePerLifetimeScope();

        builder.RegisterType<CommandsScheduler>()
            .As<ICommandScheduler>()
            .InstancePerLifetimeScope();

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerDecorator<>),
            typeof(ICommandHandler<>));

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));
    }
}