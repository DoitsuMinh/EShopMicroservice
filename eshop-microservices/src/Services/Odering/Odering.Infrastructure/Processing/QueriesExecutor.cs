using Autofac;
using MediatR;
using Ordering.Application.Configuration.CQRS.Queries;

namespace Odering.Infrastructure.Processing;

public static class QueriesExecutor
{
    public static async Task<T> Execute<T>(IQuery<T> query)
    {
        using (var scope = CompositionRoot.BeginLifeTimeScope())
        {
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(query);
        }
    }
}
