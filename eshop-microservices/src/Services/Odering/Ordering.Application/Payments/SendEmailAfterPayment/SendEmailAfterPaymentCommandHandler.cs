using MediatR;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Application.Configuration.Emails;
using Ordering.Domain.Payments;

namespace Ordering.Application.Payments.SendEmailAfterPayment
{
    public class SendEmailAfterPaymentCommandHandler : ICommandHandler<SendEmailAfterPaymentCommand, Unit> 
    {
        private readonly IEmailSender _emailSender;
        private readonly IPaymentRepository _paymentRepository;
        //private readonly IUnitOfWork _uow;

        public SendEmailAfterPaymentCommandHandler(IEmailSender emailSender, IPaymentRepository paymentRepository)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        }

        public async Task<Unit> Handle(SendEmailAfterPaymentCommand request, CancellationToken cancellationToken)
        {
            // Logic of preparing an email. This is only mock.
            var emailMessage = new EmailMessage("dminhvu99@gmail.com", "dminhvu1999@gmail.com", "subject", "<p>content</p>");

            //await _emailSender.SendEmailAsync(emailMessage);

            var payment = await _paymentRepository.GetByIdAsync(request.PaymentId);

            payment.MarkEmailNotificationAsSent();

            //await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
