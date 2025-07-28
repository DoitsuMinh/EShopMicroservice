using Ordering.Domain.Customers.Orders;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Payments;

public class Payment: Entity, IAggregateRoot
{
    public PaymentId Id { get; private set; }
    private OrderId _orderId;
    private DateTime _createdDate;
    private PaymentStatus _status;
    private bool _emailNotificationIsSent;

    private Payment()
    {
    }

    public Payment(OrderId orderId)
    {
        Id = new PaymentId(Guid.NewGuid());
        _createdDate = DateTime.UtcNow;
        _orderId = orderId;
        _status = PaymentStatus.ToPay;
        _emailNotificationIsSent = false;

        AddDomainEvent(new PaymentCreatedEvent(Id, _orderId));
    }

    public void MarkEmailNotificationAsSent()
    {
        _emailNotificationIsSent = true;
    }
}
