using Newtonsoft.Json;
using Ordering.Application.Configuration.DomainEvents;
using Ordering.Domain.Payments;

namespace Ordering.Application.Payments;

public class PaymentCreatedNotification : DomainNotificationBase<PaymentCreatedEvent>
{
    public PaymentId PaymentId { get; }

    public PaymentCreatedNotification(PaymentCreatedEvent domainEvent) : base(domainEvent)
    {
        PaymentId = domainEvent.PaymentId;
    }

    [JsonConstructor]
    public PaymentCreatedNotification(PaymentId paymentId) : base(null)
    {
        PaymentId = paymentId;
    }
}
