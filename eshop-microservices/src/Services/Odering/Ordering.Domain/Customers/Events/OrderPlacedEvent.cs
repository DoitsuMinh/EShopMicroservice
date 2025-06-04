using Ordering.Domain.Customers.Orders;
using Ordering.Domain.SeedWork;
using Ordering.Domain.Shared.MoneyValue;

namespace Ordering.Domain.Customers.Events;

public class OrderPlacedEvent : DomainEventBase
{
    public OrderPlacedEvent(OrderId orderId, CustomerId customerId, MoneyValue value)
    {
        OrderId = orderId;
        CustomerId = customerId;
        Value = value;
    }

    public OrderId OrderId { get; }
    public CustomerId CustomerId { get; }
    public MoneyValue Value { get; }
}