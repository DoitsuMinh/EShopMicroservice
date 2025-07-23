using MediatR;
using Odering.Infrastructure.Processing.Outbox;
using Ordering.Application.Configuration.CQRS.Commands;

namespace Odering.Infrastructure.Processing.InternalCommands;

internal class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
{
}
