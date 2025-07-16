using Ordering.Domain.Customers;
using Ordering.Domain.Customers.Events;
using Ordering.Domain.Customers.Orders;
using Ordering.Domain.Customers.Rules;
using Ordering.Domain.ForeignExchange;
using Ordering.Domain.Products;
using Ordering.Domain.Shared;
using Ordering.Domain.Shared.MoneyValue;
using Ordering.UnitTests.SeedWork;

namespace Ordering.UnitTests.Customers;

[TestFixture]
public class PlaceOrderTests : TestBase
{
    private readonly string AUD_CURRENCY = "AUD";
    [Test]
    public void PlaceOrder_WhenAtLeastOneProductIsAdded_IsSuccessful()
    {
        // Arange
        Customer customer = CustomerFactory.Create();

        var orderProductsData = new List<OrderProductData>()
        {
            new OrderProductData(SampleProducts.Product1Id, quantity: 2)
        };

        var allProductPrices = new List<ProductPriceData>
        {
            SampleProductPrices.Product1AUD, SampleProductPrices.Product2USD
        };

        var conversionRates = GetConversionRates();

        // Act
        customer.PlacedOrder(
            orderProductsData,
            allProductPrices,
            AUD_CURRENCY,
            conversionRates);

        // Assert
        var orderPlaced = AssertPublishedDomainEvent<OrderPlacedEvent>(customer);
        var expectedMoneyValue = MoneyValue.Of(200, "AUD");
        Assert.That(
            orderPlaced.Value, 
            Is.EqualTo(expectedMoneyValue), 
            message: $"Expected is {expectedMoneyValue.Value} {expectedMoneyValue.Currency} but result is {orderPlaced.Value.Value} {orderPlaced.Value.Currency}");

    }

    [Test]
    public void PlaceOrder_WhenNoProductsAreAdded_ThrowsOrderMustHaveAtLeastOneProductRule()
    {
        // Arrange
        Customer customer = CustomerFactory.Create();
        var orderProductsData = new List<OrderProductData>();
        //orderProductsData.Add(new OrderProductData(SampleProducts.Product1Id, quantity: 2));
        var allProductPrices = new List<ProductPriceData>
        {
            SampleProductPrices.Product1AUD, SampleProductPrices.Product2USD
        };
        var conversionRates = GetConversionRates();
        // Assert
        AssertBrokenRule<OrderMustHaveAtLeastOneProductRule>(() =>
        {
            customer.PlacedOrder(
                orderProductsData,
                allProductPrices,
                AUD_CURRENCY,
                conversionRates);
        });
    }

    [Test]
    public void PlaceOrder_WhenCustomerHasMoreThan20OrdersOnTheSameDay_ThrowsCustomerCannotOrderMoreThan20OrdersOnTheSameDayRule()
    {
        // Arrange
        Customer customer = CustomerFactory.Create();
        var orderProductsData = new List<OrderProductData>()
        {
            new OrderProductData(SampleProducts.Product1Id, quantity: 2)
        };
        var allProductPrices = new List<ProductPriceData>
        {
            SampleProductPrices.Product1AUD, SampleProductPrices.Product2USD
        };
        var conversionRates = GetConversionRates();
        // Simulate 20 orders on the same day
        SystemClock.Set(new DateTime(2025, 07, 16, 15, 54, 0));
        for (int i = 0; i < 20; i++)
        {
            customer.PlacedOrder(
                orderProductsData,
                allProductPrices,
                AUD_CURRENCY,
                conversionRates);
        }
        // Assert
        AssertBrokenRule<CustomerCannotOrderMoreThan20OrdersOnTheSameDayRule>(() =>
        {
            customer.PlacedOrder(
                orderProductsData,
                allProductPrices,
                AUD_CURRENCY,
                conversionRates);
        });
    }

    //[Test]
    //public void PlaceOrder_GivenTwoOrdersInThatDayAlreadyMade_BreaksCustomerCannotOrderMoreThan2OrdersOnTheSameDayRule()
    //{
    //    // Arrange
    //    var customer = CustomerFactory.Create();

    //    var orderProductsData = new List<OrderProductData>();
    //    orderProductsData.Add(new OrderProductData(SampleProducts.Product1Id, 2));

    //    var allProductPrices = new List<ProductPriceData>
    //        {
    //            SampleProductPrices.Product1AUD, SampleProductPrices.Product2USD
    //        };

    //    var conversionRates = GetConversionRates();

    //    SystemClock.Set(new DateTime(2025, 07, 16, 15, 54, 0));
    //    customer.PlacedOrder(
    //        orderProductsData,
    //        allProductPrices,
    //        AUD_CURRENCY,
    //        conversionRates);

    //    SystemClock.Set(new DateTime(2025, 07, 16, 16, 0, 0));
    //    customer.PlacedOrder(
    //        orderProductsData,
    //        allProductPrices,
    //        AUD_CURRENCY,
    //        conversionRates);

    //    SystemClock.Set(new DateTime(2025, 07, 17, 0, 00, 0));

    //    // Assert
    //    AssertBrokenRule<CustomerCannotOrderMoreThan20OrdersOnTheSameDayRule>(() =>
    //    {
    //        // Act
    //        customer.PlacedOrder(
    //            orderProductsData,
    //            allProductPrices,
    //            AUD_CURRENCY,
    //            conversionRates);
    //    });
    //}

    private static List<ConversionRate> GetConversionRates()
    {
        return new List<ConversionRate>
        {
            new ConversionRate("USD", "AUD", 1.52m),
            new ConversionRate("AUD", "USD", 0.66m),
        };
    }
}

public class SampleProducts
{
    public static readonly ProductId Product1Id = new ProductId(Guid.NewGuid());
    public static readonly ProductId Product2Id = new ProductId(Guid.NewGuid());
}

public class SampleProductPrices
{
    public static readonly ProductPriceData Product1AUD = new ProductPriceData(
        SampleProducts.Product1Id,
        MoneyValue.Of(100, "AUD"));

    public static readonly ProductPriceData Product2USD = new ProductPriceData(
        SampleProducts.Product2Id,
        MoneyValue.Of(100, "USD"));
}