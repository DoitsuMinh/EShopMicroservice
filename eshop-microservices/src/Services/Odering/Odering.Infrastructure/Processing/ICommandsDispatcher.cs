namespace Odering.Infrastructure.Processing;

public interface ICommandsDispatcher
{
    Task DispatchCommandAsync(Guid Id);
}