using Ordering.Domain.ForeignExchange;
using Ordering.Domain.Products;
using Ordering.Domain.SeedWork;
using Ordering.Domain.Shared.MoneyValue;

namespace Ordering.Domain.Customers.Orders;

public class OrderProduct: Entity
{
    public int Quantity { get; private set; }
    public ProductId ProductId { get; private set; }
    public MoneyValue Value { get; private set; }
    public MoneyValue ValueInAUD { get; private set; }

    private OrderProduct()
    {
        // EF Core
    }

    private OrderProduct(
        ProductPriceData productPrice,
        int quantity,
        string currency,
        List<ConversionRate> conversionRates)
    {
        ProductId = productPrice.ProductId;
        Quantity = quantity;
        CalculateValue(productPrice, currency, conversionRates);
    }

    private void CalculateValue(ProductPriceData productPrice, string currency, List<ConversionRate> conversionRates)
    {

        Value = Quantity * productPrice.Price;
        if (currency == "AUD")
        {
            ValueInAUD = Quantity * productPrice.Price;
        } else
        {
            var conversionRate = conversionRates.Single(x => x.SourceCurrency == currency && x.TargetCurrency == "AUD");
            ValueInAUD = conversionRate.Convert(Value);
        }
    }

    internal static OrderProduct CreateForProduct(ProductPriceData productPrice, int quantity, string currency, List<ConversionRate> conversionRates)
    {
        return new OrderProduct(productPrice, quantity, currency, conversionRates);
    }

    internal void ChangeQuantity(ProductPriceData product, int quantity, List<ConversionRate> conversionRates)
    {
        Quantity = quantity;
        CalculateValue(product, Value.Currency, conversionRates);
    }
}
