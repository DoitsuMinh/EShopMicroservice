using Ordering.Application.Configuration.CQRS.Commands;

namespace Ordering.Application.Orders.PlaceCustomerOrders;

public class PlaceCustomerOrderCommand : CommandBase<Guid>
{
    public Guid CustomerId { get; }
    public List<ProductDto> Products { get; }
    public string Currency { get; }

    public PlaceCustomerOrderCommand(
        Guid customerId,
        List<ProductDto> products,
        string currency)
    {
        CustomerId = customerId;
        Products = products;
        Currency = currency;
    }
}
