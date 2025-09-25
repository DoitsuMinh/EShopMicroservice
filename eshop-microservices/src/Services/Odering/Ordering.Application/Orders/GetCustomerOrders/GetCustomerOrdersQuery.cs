using Ordering.Application.Configuration.CQRS.Queries;

namespace Ordering.Application.Orders.GetCustomerOrders;

public class GetCustomerOrdersQuery : IQuery<List<OrderDto>>
{
    public Guid CustomerId { get; }
    public GetCustomerOrdersQuery(Guid customerId)
    {
        CustomerId = customerId;
    }
}
