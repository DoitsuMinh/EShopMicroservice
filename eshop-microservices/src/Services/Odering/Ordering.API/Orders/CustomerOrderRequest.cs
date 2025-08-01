using Ordering.Application.Orders;

namespace Ordering.API.Orders;

public class CustomerOrderRequest
{
    public List<ProductDto> Products { get; set; }
    public string Currency { get; set; }
}