using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers;

public class CustomerRegisteredEvent : DomainEventBase
{
    public CustomerId CustomerId { get; }
    public CustomerRegisteredEvent(CustomerId customerId)
    {
        CustomerId = customerId;
    }
}