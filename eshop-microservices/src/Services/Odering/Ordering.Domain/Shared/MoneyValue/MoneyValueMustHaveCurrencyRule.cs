using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Shared.MoneyValue;

public class MoneyValueMustHaveCurrencyRule : IBusinessRule
{
    private readonly string _currency;

    public MoneyValueMustHaveCurrencyRule(string currency)
    {
        _currency = currency;
    }

    public string Message => "Money value must have currency.";

    public bool IsBroken()
    {
        return _currency.IsNullOrEmptyOrWhiteSpace() || _currency.Length != 3;
    }
}