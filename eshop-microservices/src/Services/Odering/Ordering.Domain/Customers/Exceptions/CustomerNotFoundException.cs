namespace Ordering.Domain.Customers.Exceptions;

public class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(string message) : base(message) { }
    public CustomerNotFoundException(object key)
        : base($"Customer {key.ToString()} was not found") { }
}
