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
        return new MoneyValue(value, currency);
    }

    public static MoneyValue Of(MoneyValue value)
    {
        return new MoneyValue(value.Value, value.Currency);
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
}

public static class SumExtensions
{
    /// <summary>
    /// Sums a collection of MoneyValue objects using a selector function.
    /// Like lambda in python
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static MoneyValue Sum<T>(this IEnumerable<T> src, Func<T, MoneyValue> selector)
    {
        return MoneyValue.Of(src.Select(selector).Aggregate((x, y) => x + y));
    }

    public static MoneyValue Sum(this IEnumerable<MoneyValue> src)
    {
        return src.Aggregate((x, y) => x + y);
    }
}
