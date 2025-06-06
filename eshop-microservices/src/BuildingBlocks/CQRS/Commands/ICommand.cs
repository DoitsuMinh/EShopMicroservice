using MediatR;

namespace BuildingBlocks.CQRS.Commands;

public interface ICommand : ICommand<Unit>
{

}

public interface ICommand<out TResponse> : IRequest<TResponse>
{

}

