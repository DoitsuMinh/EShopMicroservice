using Ordering.Domain.Customers.Orders;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Payments;

public class PaymentCreatedEvent : DomainEventBase
{
    public PaymentCreatedEvent(PaymentId paymentId, OrderId orderId)
    {
        PaymentId = paymentId;
        OrderId = orderId;
    }

    public OrderId OrderId { get; set; }
    public PaymentId PaymentId { get; set; }
}
