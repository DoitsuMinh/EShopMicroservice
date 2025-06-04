using Ordering.Domain.Customers.Orders;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers.Events;

public class OrderChangedEvent : DomainEventBase
{
    public OrderChangedEvent(OrderId orderId)
    {
        OrderId = orderId;
    }
    public OrderId OrderId { get; }
}