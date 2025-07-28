namespace Ordering.Domain.Payments;

public interface IPaymentRepository
{
    Task<Payment> GetByIdAsync(PaymentId paymentId);
    Task AddAsync(Payment payment);
}
