using MediatR;
using Ordering.Domain.Customers.Events;
using Ordering.Domain.Payments;

namespace Ordering.Application.Orders.PlaceCustomerOrders;

public class OrderPlacedDomainEventHandler : INotificationHandler<OrderPlacedEvent>
{
    private readonly IPaymentRepository _paymentRepository;

    public OrderPlacedDomainEventHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
    {
        var newPayment = new Payment(notification.OrderId);

        await _paymentRepository.AddAsync(newPayment);
    }
}
