using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Products;

public class ProductId(Guid value) : TypedIdValueBase(value)
{
}

// Equal this
//public class ProductId : TypedIdValueBase
//{
//    public ProductId(Guid value) : base(value)
//    {
//    }
//}