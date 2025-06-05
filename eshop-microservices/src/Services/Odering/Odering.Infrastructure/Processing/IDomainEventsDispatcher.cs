namespace Odering.Infrastructure.Processing;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}