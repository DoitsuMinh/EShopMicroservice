namespace Ordering.Domain.Customers.Exceptions;

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(string message)
        : base(message)
    {
    }
    public OrderNotFoundException(object key)
        : base($"Order {key.ToString()} was not found.")
    {
    }
}
