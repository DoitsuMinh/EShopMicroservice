using Autofac;
using MediatR;
using Odering.Infrastructure.Logging;
using Odering.Infrastructure.Processing.InternalCommands;
using Ordering.Application.Configuration.CQRS.Commands;

namespace Odering.Infrastructure.Processing;

public class ProcessingModule : Module
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

        builder.RegisterGenericDecorator(
            typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
            typeof(INotificationHandler<>));

        // TODO: PaymentCreatedNotification
        //
        //

        var serviice = new List<string>();

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerDecorator<>),
            typeof(ICommandHandler<>));

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));

        builder.RegisterType<CommandsDispatcher>()
            .As<ICommandsDispatcher>()
            .InstancePerLifetimeScope();

        // TODO: RegisterType CommandsScheduler
        //
        //

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerDecorator<>),
            typeof(ICommandHandler<>));

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));
    }
}