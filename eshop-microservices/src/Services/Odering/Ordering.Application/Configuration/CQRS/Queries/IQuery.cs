using MediatR;

namespace Ordering.Application.Configuration.CQRS.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{

}