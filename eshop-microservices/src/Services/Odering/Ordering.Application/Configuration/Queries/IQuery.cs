using MediatR;

namespace Ordering.Application.Configuration.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{

}