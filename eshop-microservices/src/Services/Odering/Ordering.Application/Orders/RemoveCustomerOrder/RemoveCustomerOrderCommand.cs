using MediatR;
using Ordering.Application.Configuration.CQRS.Commands;

namespace Ordering.Application.Orders.RemoveCustomerOrder;

public class RemoveCustomerOrderCommand :  CommandBase<Unit>
{
    public Guid CustomerId { get; }
    public Guid OrderId { get; }
    public RemoveCustomerOrderCommand(Guid customerId, Guid orderId)
    {
        CustomerId = customerId;
        OrderId = orderId;
    }
}
