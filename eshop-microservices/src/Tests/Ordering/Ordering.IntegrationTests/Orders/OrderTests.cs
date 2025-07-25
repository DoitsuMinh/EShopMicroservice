using Odering.Infrastructure.Processing;
using Ordering.Application.Customers.RegisterCustomer;
using Ordering.Application.Orders;
using Ordering.Application.Orders.GetCustomerOrderDetails;
using Ordering.Application.Orders.PlaceCustomerOrders;
using Ordering.IntegrationTests.SeedWork;

namespace Ordering.IntegrationTests.Orders;

[TestFixture]
public class OrderTests : TestBase
{
    [Test]
    public async Task PlaceOrder_Test()
    {
        const string TEST_EMAIL = "newcustomer@mail.com";
        const string TEST_NAME = "Test Customer";
        const string AUD_CURRENCY = "AUD";
        const int TEST_QTY = 100;
        var customer = await CommandsExecutor.Execute(new RegisterCustomerCommand(TEST_EMAIL, TEST_NAME));

        var products = new List<ProductDto>();
        var productId = Guid.Parse("9db6e474-ae74-4cf5-a0dc-ba23a42e2566");

        products.Add(new ProductDto(productId, TEST_QTY));
        var orderId = await CommandsExecutor.Execute(
            new PlaceCustomerOrderCommand(customer.Id, products, AUD_CURRENCY));

        var orderDetails = await QueriesExecutor.Execute(new GetCustomerOrderDetailsQuery(orderId));

        Assert.That(orderDetails, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(orderDetails.Value, Is.EqualTo(6157m));
            Assert.That(orderDetails.Currency, Is.EqualTo(AUD_CURRENCY));
            Assert.That(orderDetails.Products.Count, Is.EqualTo(1));
            Assert.That(orderDetails.Products[0].Quantity, Is.EqualTo(TEST_QTY));
            Assert.That(orderDetails.Products[0].Id, Is.EqualTo(productId));
        });
    }
}
