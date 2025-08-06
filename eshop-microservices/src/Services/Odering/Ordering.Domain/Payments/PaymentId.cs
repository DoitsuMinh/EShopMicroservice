using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Payments;

public class PaymentId : TypedIdValueBase
{
    public PaymentId(Guid value) : base(value)
    {
    }
}
