namespace Ordering.Application.Orders.GetCustomerOrders;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; }
    public string IsRemoved { get; set; }
}
