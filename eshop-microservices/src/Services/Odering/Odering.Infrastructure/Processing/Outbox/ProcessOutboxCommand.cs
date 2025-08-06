using MediatR;
using Ordering.Application.Configuration.CQRS.Commands;

namespace Odering.Infrastructure.Processing.Outbox;

public class ProcessOutboxCommand : CommandBase<Unit>, IRecurringCommand
{

}
