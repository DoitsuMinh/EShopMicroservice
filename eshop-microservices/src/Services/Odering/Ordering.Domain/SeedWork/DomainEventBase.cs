namespace Ordering.Domain.SeedWork;

public class DomainEventBase: IDomainEvent
{
    public DomainEventBase()
    {
        OccuredOn = DateTime.Now;
    }

    public DateTime OccuredOn { get; }
}
