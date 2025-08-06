using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers.Exceptions;

public class ProductOrderNotFoundException : NotFoundException
{
    public ProductOrderNotFoundException(object key)
        : base("Product Order", key)
    {
    }
}