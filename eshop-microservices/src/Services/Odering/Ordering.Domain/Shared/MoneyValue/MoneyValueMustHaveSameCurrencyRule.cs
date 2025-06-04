using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Shared.MoneyValue;

public class MoneyValueMustHaveSameCurrencyRule : IBusinessRule
{
    private readonly MoneyValue _left;
    private readonly MoneyValue _right;

    public MoneyValueMustHaveSameCurrencyRule(MoneyValue left, MoneyValue right)
    {
        _left = left;
        _right = right;
    }
    public string Message => "Money value must have same currency.";

    public bool IsBroken()
    {
        return _left.Currency != _right.Currency;
    }
}