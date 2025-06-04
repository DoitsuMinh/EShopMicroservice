using Ordering.Domain.SeedWork;
using Ordering.Domain.Shared.MoneyValue;

namespace Ordering.Domain.Products;

public class ProductPriceData: ValueObject
{
    public ProductPriceData(ProductId productId, MoneyValue moneyValue)
    {
        ProductId = productId;
        Price = moneyValue;
    }

    public ProductId ProductId { get; }
    public MoneyValue Price { get; }
}
