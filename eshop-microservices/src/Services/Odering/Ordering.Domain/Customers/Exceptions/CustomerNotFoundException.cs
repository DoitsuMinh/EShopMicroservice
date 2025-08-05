using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers.Exceptions;

public class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(object key)
        : base("Customer", key) { }
}
