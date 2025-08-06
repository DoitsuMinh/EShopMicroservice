using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers.Exceptions;

public class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(object key)
        : base("Order", key)
    {
    }
}
