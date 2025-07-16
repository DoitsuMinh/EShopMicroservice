using NSubstitute;
using Ordering.Domain.Customers;
using Ordering.Domain.Customers.Rules;
using Ordering.UnitTests.SeedWork;

namespace Ordering.UnitTests.Customers;

[TestFixture]
public class CustomerRegistrationTests: TestBase
{
    [Test]
    public void GivenCustomerEmailIsUnique_WhenCustomerisRegistering_IsSuccessful()
    {
        // Arrange
        var customerUniquenessChecker = Substitute.For<ICustomerUniquenessChecker>();
        const string email = "testCustomer@mail.com";
        customerUniquenessChecker.IsUnique(email).Returns(true);
        // Act
        var customer = Customer.CreateRegistered(email, "Test Name", customerUniquenessChecker);
        // Assert
        AssertPublishedDomainEvent<CustomerRegisteredEvent>(customer);
    }

    [Test]
    public void GivenCustomerEmailIsNotUnique_WhenCustomerisRegistering_ThrowsCustomerEmailMustBeUniqueRule()
    {
        // Arrange
        var customerUniquenessChecker = Substitute.For<ICustomerUniquenessChecker>();
        const string email = "testCustomer@mail.com";
        customerUniquenessChecker.IsUnique(email).Returns(false);
        // Assert
        AssertBrokenRule<CustomerEmailMustBeUniqueRule>(() =>
        {
            Customer.CreateRegistered(email, "Test Name", customerUniquenessChecker);
        });
    }
}
