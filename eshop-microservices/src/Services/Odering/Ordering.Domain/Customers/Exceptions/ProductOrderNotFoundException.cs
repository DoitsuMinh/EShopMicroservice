namespace Ordering.Domain.Customers.Exceptions;

public class ProductOrderNotFoundException :Exception
{
    public ProductOrderNotFoundException(string message)
        : base(message)
    {
    }

    public ProductOrderNotFoundException(object key)
        : base($"Product order {key.ToString()} not found.")
    {
    }
}