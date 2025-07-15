using Odering.Infrastructure.Processing;
using Ordering.Application.Customers.GetCustomerDetails;
using Ordering.Application.Customers.RegisterCustomer;
using Ordering.IntegrationTests.SeedWork;

namespace Ordering.IntegrationTests.Customers;

public class CustomersTests : TestBase
{
    [Test]
    public async Task RegisterCustomerTest()
    {
        const string email = "newcustomer@mail.com";
        const string name = "Test Customer";

        var customer = await CommandsExecutor.Execute(new RegisterCustomerCommand(email, name));
        var customerDetails = await QueriesExecutor.Execute(new GetCustomerDetailsQuery(customer.Id));

        Assert.That(customerDetails, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(customerDetails.Email, Is.EqualTo(email));
            Assert.That(customerDetails.Name, Is.EqualTo(name));
        });

    }
}
