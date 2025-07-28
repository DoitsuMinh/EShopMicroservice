using Microsoft.EntityFrameworkCore;
using Odering.Infrastructure.Database;
using Ordering.Domain.Payments;

namespace Odering.Infrastructure.Domain.Payments;

public class PaymentRepository : IPaymentRepository
{
    private readonly OrdersContext _ordersContext;
    public PaymentRepository(OrdersContext ordersContext)
    {
        _ordersContext = ordersContext ?? throw new ArgumentNullException(nameof(ordersContext));
    }

    public async Task AddAsync(Payment payment)
    {
        await _ordersContext.Payments.AddAsync(payment);
    }

    public async Task<Payment> GetByIdAsync(PaymentId Id)
    {
        return await _ordersContext.Payments.SingleAsync(x => x.Id == Id);
    }
}
