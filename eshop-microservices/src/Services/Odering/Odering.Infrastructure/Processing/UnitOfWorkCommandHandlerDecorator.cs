﻿using Microsoft.EntityFrameworkCore;
using Odering.Infrastructure.Database;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Domain.SeedWork;

namespace Odering.Infrastructure.Processing;

public class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ICommandHandler<T> _decorated;

    private readonly IUnitOfWork _unitOfWork;

    private readonly OrdersContext _ordersContext;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T> decorated,
        IUnitOfWork unitOfWork,
        OrdersContext ordersContext)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _ordersContext = ordersContext;
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase)
        {
            var internalCommand =
                   await _ordersContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

            if (internalCommand != null)
            {
                internalCommand.ProcessedDate = DateTime.UtcNow;
            }
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }    
}
