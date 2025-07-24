using Newtonsoft.Json;
using Ordering.Domain.SeedWork;

namespace Ordering.Application.Configuration.DomainEvents;

public class DomainNotificationBase<T> : IDomainEventNotification<T> where T : IDomainEvent
{
    [JsonIgnore]
    public T DomainEvent { get; }

    public Guid Id { get; }

    public DomainNotificationBase(T domainEvent)
    {
        DomainEvent = domainEvent;
        Id = Guid.NewGuid();
    }
}
