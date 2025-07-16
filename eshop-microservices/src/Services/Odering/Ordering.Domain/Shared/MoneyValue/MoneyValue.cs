using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Shared.MoneyValue;

public class MoneyValue : ValueObject
{
    public decimal Value { get; }
    public string Currency { get; }

    private MoneyValue(decimal value, string currency)
    {
        Value = value;
        Currency = currency;
    }

    public static MoneyValue Of(decimal value, string currency)
    {
        CheckRule(new MoneyValueMustHaveCurrencyRule(currency));
        
        var formattedValue = RoundToThreeDecimalPlacesForCurrency(value);

        //CheckRule(new MoneyValueMustBeRoundedToThreeDecimalPlacesRule(formattedValue));

        return new MoneyValue(formattedValue, currency);
    }

    public static MoneyValue Of(MoneyValue value)
    {
        var formattedValue = RoundToThreeDecimalPlacesForCurrency(value.Value);
        //CheckRule(new MoneyValueMustBeRoundedToThreeDecimalPlacesRule(formattedValue));
        return new MoneyValue(formattedValue, value.Currency);
    }

    public static MoneyValue operator +(MoneyValue left, MoneyValue right)
    {
        CheckRule(new MoneyValueMustHaveSameCurrencyRule(left, right));

        return new MoneyValue(left.Value + right.Value, left.Currency);
    }

    public static MoneyValue operator *(int number, MoneyValue right)
    {
        return new MoneyValue(number * right.Value, right.Currency);
    }

    public static MoneyValue operator *(decimal number, MoneyValue right)
    {
        return new MoneyValue(number * right.Value, right.Currency);
    }

    private static decimal RoundToThreeDecimalPlacesForCurrency(decimal value)
    {
        return Math.Round(value, 3, MidpointRounding.AwayFromZero);
    }
}

public static class SumExtensions
{
    /// <summary>
    /// Sums a collection of MoneyValue objects using a selector function.
    /// </summary>
    public static MoneyValue Sum<T>(this IEnumerable<T> src, Func<T, MoneyValue> selector)
    {
        return MoneyValue.Of(src.Select(selector).Aggregate((x, y) => x + y));
    }

    public static MoneyValue Sum(this IEnumerable<MoneyValue> src)
    {
        return src.Aggregate((x, y) => x + y);
    }
}
