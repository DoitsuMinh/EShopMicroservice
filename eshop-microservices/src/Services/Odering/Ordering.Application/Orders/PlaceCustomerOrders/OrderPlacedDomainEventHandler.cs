using MediatR;
using Ordering.Domain.Customers.Events;

namespace Ordering.Application.Orders.PlaceCustomerOrders;

public class OrderPlacedDomainEventHandler : INotificationHandler<OrderPlacedEvent>
{
    public OrderPlacedDomainEventHandler()
    {
        
    }

    public Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("This is a placeholder for the actual email sending logic.");
    }
}
