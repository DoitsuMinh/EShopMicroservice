using Newtonsoft.Json;
using Ordering.Application.Configuration.DomainEvents;
using Ordering.Domain.Customers;
using Ordering.Domain.Customers.Events;
using Ordering.Domain.Customers.Orders;

namespace Ordering.Application.Orders.PlaceCustomerOrders;

public class OrderPlacedNotification : DomainNotificationBase<OrderPlacedEvent>
{
    public OrderId OrderId { get; }
    public CustomerId CustomerId { get; }

    public OrderPlacedNotification(OrderPlacedEvent domainEvent) : base(domainEvent)
    {
        OrderId = domainEvent.OrderId;
        CustomerId = domainEvent.CustomerId;
    }

    [JsonConstructor]
    public OrderPlacedNotification(OrderId orderId, CustomerId customerId) : base(null)
    {
        OrderId = orderId;
        CustomerId = customerId;
    }
}
