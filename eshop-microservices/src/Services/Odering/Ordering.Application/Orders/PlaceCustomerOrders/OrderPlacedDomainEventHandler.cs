using MediatR;
using Ordering.Domain.Customers.Events;

namespace Ordering.Application.Orders.PlaceCustomerOrders;

public class OrderPlacedDomainEventHandler : INotificationHandler<OrderPlacedEvent>
{
    //private readonly IPaymentRepository _paymentRepository;

    public OrderPlacedDomainEventHandler(
        //IPaymentRepository paymentRepository
        )
    {
        //_paymentRepository = paymentRepository;
    }

    public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
    {
        //var newPayment = new Payment(notification.OrderId);

        //await this._paymentRepository.AddAsync(newPayment);
    }
}
