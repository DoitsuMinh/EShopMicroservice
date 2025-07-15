using Ordering.Domain.Shared.MoneyValue;
using Ordering.UnitTests.SeedWork;

namespace Ordering.UnitTests.SharedKernel;

[TestFixture]
public class MoneyValueTests : TestBase
{
    [Test]
    public void MoneyValueOf_WhenCurrencyIsProvided_IsSuccessful()
    {
        // Arrange
        var amount = 100m;
        var currency = "USD";
        // Act
        var moneyValue = MoneyValue.Of(amount, currency);
        // Assert
        Assert.That(moneyValue.Value, Is.EqualTo(amount));
        Assert.That(moneyValue.Currency, Is.EqualTo(currency));
    }
    
}
