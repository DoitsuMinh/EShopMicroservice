using MediatR;

namespace Ordering.Application.Configuration.CQRS.Queries;

public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}
