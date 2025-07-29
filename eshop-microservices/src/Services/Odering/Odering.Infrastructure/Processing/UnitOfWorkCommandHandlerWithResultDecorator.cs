using Microsoft.EntityFrameworkCore;
using Odering.Infrastructure.Database;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Domain.SeedWork;

namespace Odering.Infrastructure.Processing;

public class UnitOfWorkCommandHandlerWithResultDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _decorated;

    private readonly IUnitOfWork _unitOfWork;

    private readonly OrdersContext _ordersContext;

    public UnitOfWorkCommandHandlerWithResultDecorator(ICommandHandler<TCommand, TResult> decorated, IUnitOfWork unitOfWork, OrdersContext ordersContext)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _ordersContext = ordersContext;
    }

    public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
    {
        // write to console for debugging purposes
        Console.WriteLine($"UnitOfWorkCommandHandlerWithResultDecorator triggered for: {command.GetType().Name}");

        var result = await _decorated.Handle(command, cancellationToken);

        if(command is InternalCommandBase<TResult>)
        {
            var internalCommand =
                await _ordersContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);
            if (internalCommand != null)
            {
                internalCommand.ProcessedDate = DateTime.UtcNow;
            }
        }
        await _unitOfWork.CommitAsync(cancellationToken);

        return result;
    }
}
