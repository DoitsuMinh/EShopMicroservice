using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Odering.Infrastructure.Database;
using Ordering.Application.Customers;
using System.Reflection;

namespace Odering.Infrastructure.Processing.InternalCommands;

internal class CommandsDispatcher : ICommandsDispatcher
{
    private readonly IMediator _mediator;
    private readonly OrdersContext _ordersContext;

    public CommandsDispatcher(IMediator mediator, OrdersContext ordersContext)
    {
        _mediator = mediator;
        _ordersContext = ordersContext;
    }

    public async Task DispatchCommandAsync(Guid Id)
    {
        var internalCommand = await _ordersContext.InternalCommands.SingleOrDefaultAsync(x => x.Id == Id);

        Type type = Assembly.GetAssembly(typeof(MarkCustomerAsWelcomedCommand)).GetType(internalCommand.Type);
        dynamic command = JsonConvert.DeserializeObject(internalCommand.Data, type);

        internalCommand.ProcessedDate = DateTime.UtcNow;

        await this._mediator.Send(command);

        //throw new NotImplementedException("This method is not implemented yet. Please implement the logic to dispatch commands based on the internal command ID.");
    }
}