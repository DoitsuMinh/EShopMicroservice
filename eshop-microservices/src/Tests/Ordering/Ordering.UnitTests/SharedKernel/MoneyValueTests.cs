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
        var amount = 1m;
        var currency = "AUD";
        // Act
        var moneyValue = MoneyValue.Of(amount, currency);
        // Assert
        Assert.That(moneyValue.Value, Is.EqualTo(amount));
        Assert.That(moneyValue.Currency, Is.EqualTo(currency));
    }

    [Test]
    public void MoneyValueOf_WhenCurrencyIsNotProvided_ThrowsMoneyValueMustHaveCurrencyRule()
    {
        AssertBrokenRule<MoneyValueMustHaveCurrencyRule>(() => MoneyValue.Of(1m, ""));
    }

    [Test]
    public void GivenTwoMoneyValuesWithSameCurrency_WhenAddThem_IsSuccessful()
    {
        // Arrage
        var valueInAUD = MoneyValue.Of(100m, "AUD");
        var valueInAUD2 = MoneyValue.Of(50m, "AUD");
        // Act
        MoneyValue result = valueInAUD + valueInAUD2;
        // Assert
        Assert.That(result.Value, Is.EqualTo(150m));
        Assert.That(result.Currency, Is.EqualTo("AUD"));
    }

    [Test]
    public void GivenListMoneyValues_WhenSumThem_IsSuccessful()
    {
        // Arrange
        var moneyValue1 = MoneyValue.Of(100m, "AUD");
        var moneyValue2 = MoneyValue.Of(50m, "AUD");
        var moneyValue3 = MoneyValue.Of(25m, "AUD");
        IList<MoneyValue> moneyValues = new List<MoneyValue>
        {
            moneyValue1, moneyValue2, moneyValue3
        };
        // Act
        MoneyValue result = moneyValues.Sum();
        // Assert
        Assert.That(result.Value, Is.EqualTo(175m));
        Assert.That(result.Currency, Is.EqualTo("AUD"));
    }

    [Test]
    public void GivenTwoMoneyValuesWithDifferentCurrencies_WhenAddThem_ThrowsMoneyValueMustHaveSameCurrencyRule()
    {
        // Arrange
        MoneyValue valueInAUD = MoneyValue.Of(100m, "AUD");
        MoneyValue valueInUSD = MoneyValue.Of(50m, "USD");
        // Act & Assert
        AssertBrokenRule<MoneyValueMustHaveSameCurrencyRule>(() =>
        {
            var result = valueInAUD + valueInUSD;
        });
    }

    [Test]
    public void GivenListMoneyValuesContainsNegativeValue_WhenSumThem_IsSuccessful()
    {
        // Arrange
        var moneyValue1 = MoneyValue.Of(100.123m, "AUD");
        var moneyValue2 = MoneyValue.Of(-50.123m, "AUD");
        var moneyValue3 = MoneyValue.Of(25.001m, "AUD");
        IList<MoneyValue> moneyValues = new List<MoneyValue>
        {
            moneyValue1, moneyValue2, moneyValue3
        };
        // Act
        MoneyValue result = moneyValues.Sum();
        // Assert
        Assert.That(result.Value, Is.EqualTo(75.001m));
        Assert.That(result.Currency, Is.EqualTo("AUD"));
    }

    [TestCase(1.124, 1.124)]
    [TestCase(1.0066, 1.007)]
    [TestCase(1.76, 1.76)]
    [TestCase(-1.0009, -1.001)]
    public void MoneyValueOf_WhenValueIsRounded_ShouldReturnExpectedResult(decimal value, decimal expected)
    {
        //Arrange
        var moneyValue = MoneyValue.Of(value, "AUD");
        //Act
        var result = moneyValue.Value;
        //Assert
        Assert.That(result, Is.EqualTo(expected), 
            $"Expected {expected} but was {result}");
    }
}
