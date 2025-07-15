using Ordering.Application.Configuration.Queries;

namespace Ordering.Application.Orders.GetCustomerOrderDetails;

public class GetCustomerOrderDetailsQuery : IQuery<OrderDetailsDto>
{
    public Guid OrderId { get; }
    public GetCustomerOrderDetailsQuery(Guid orderId)
    {
        OrderId = orderId;
    }
}
