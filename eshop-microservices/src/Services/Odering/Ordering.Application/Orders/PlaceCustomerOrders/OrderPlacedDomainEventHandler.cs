using MassTransit;
using MediatR;
using Ordering.Domain.Customers.Events;
using Ordering.Domain.Payments;

namespace Ordering.Application.Orders.PlaceCustomerOrders;

public class OrderPlacedDomainEventHandler : INotificationHandler<OrderPlacedEvent>
{
    private readonly IPaymentRepository _paymentRepository;
    //private readonly IPublishEndpoint _publishEndpoint;

    public OrderPlacedDomainEventHandler(IPaymentRepository paymentRepository
        //, IPublishEndpoint publishEndpoint
        )
    {
        _paymentRepository = paymentRepository;
        //_publishEndpoint = publishEndpoint; 
    }

    public async Task Handle(OrderPlacedEvent domainEvent, CancellationToken cancellationToken)
    {
        var newPayment = new Payment(domainEvent.OrderId);

        await _paymentRepository.AddAsync(newPayment);
    }
}
