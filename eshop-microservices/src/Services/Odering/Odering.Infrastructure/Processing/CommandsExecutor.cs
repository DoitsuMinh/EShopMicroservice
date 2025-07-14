using Autofac;
using MediatR;
using Ordering.Application.Configuration.CQRS.Commands;

namespace Odering.Infrastructure.Processing;

public static class CommandsExecutor
{
    public static async Task Execute(ICommand command)
    {
        using (var scope = CompositionRoot.BeginLifeTimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(command);
        }
    }

    public static async Task<T> Execute<T>(ICommand<T> command)
    {
        using (var scope = CompositionRoot.BeginLifeTimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}
