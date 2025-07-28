using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Domain.Payments;

namespace Ordering.Application.Payments.SendEmailAfterPayment;

public class SendEmailAfterPaymentCommand : InternalCommandBase<Unit>
{
    [JsonConstructor]
    public SendEmailAfterPaymentCommand(Guid id, PaymentId paymentId): base(id)
    {
        PaymentId = paymentId;
    }

    public PaymentId PaymentId { get; }

}