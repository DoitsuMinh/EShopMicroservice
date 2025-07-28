using Ordering.Application.Configuration.CQRS.Commands;

namespace Ordering.Application.Configuration.Commands;

public interface ICommandScheduler
{
    Task EnqueueAsync<T>(ICommand<T> command);
}
