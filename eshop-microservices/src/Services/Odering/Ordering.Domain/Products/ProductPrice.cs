using Ordering.Domain.Shared.MoneyValue;

namespace Ordering.Domain.Products;

public class ProductPrice
{
    /// <summary>
    /// private mean can only be modified within the class itself. 
    /// </summary>
    public MoneyValue Value { get; private set; }

    private ProductPrice()
    { }
}
