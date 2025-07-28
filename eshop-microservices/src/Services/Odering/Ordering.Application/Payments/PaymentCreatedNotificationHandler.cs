using MediatR;
using Ordering.Application.Configuration.Commands;
using Ordering.Application.Payments.SendEmailAfterPayment;

namespace Ordering.Application.Payments
{
    public class PaymentCreatedNotificationHandler : INotificationHandler<PaymentCreatedNotification>
    {
        private readonly ICommandScheduler _commandScheduler;

        public PaymentCreatedNotificationHandler(ICommandScheduler commandScheduler)
        {
            _commandScheduler = commandScheduler;
        }

        public async Task Handle(PaymentCreatedNotification request, CancellationToken cancellationToken)
        {
            await _commandScheduler.EnqueueAsync(new SendEmailAfterPaymentCommand(Guid.NewGuid(), request.PaymentId));
        }
    }
}
