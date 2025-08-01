using MediatR;
using Ordering.Application.Configuration.CQRS.Commands;

namespace Ordering.Application.Orders.ChangeCustomerOrder;

public class ChangeCustomerOrderCommand : CommandBase<Unit>
{
    public Guid CustomerId { get; }
    public Guid OrderId { get; }
    public List<ProductDto> Products { get; }
    public string Currency { get; }

    public ChangeCustomerOrderCommand(
        Guid customerId,
        Guid orderId,
        List<ProductDto> products,
        string currency)
    {
        CustomerId = customerId;
        OrderId = orderId;
        Products = products;
        Currency = currency;
    }
}
