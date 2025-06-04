using Ordering.Domain.Customers.Orders;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers.Events;

public class OrderRemovedEvent: DomainEventBase
{
    public OrderId OrderId { get; }
    public OrderRemovedEvent(OrderId orderId)
    {
        OrderId = orderId;
    }
}
