using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Shared.MoneyValue;

public class MoneyValueMustBeRoundedToThreeDecimalPlacesRule : IBusinessRule
{
    private readonly decimal _amount;

    public MoneyValueMustBeRoundedToThreeDecimalPlacesRule(decimal ammount)
    {
        _amount = ammount;
    }

    public string Message => "Money value amount must have 3 decimal digits.";
    
    public bool IsBroken()
    {
        // Convert the decimal to string with invariant culture
        var ammountString = _amount.ToString(System.Globalization.CultureInfo.InvariantCulture);

        // Find the index of the decimal point
        var decimalPointIndex = ammountString.IndexOf('.');
        if(decimalPointIndex == -1)
        {
            return false;
        }

        // Caculate the degit after the point index
        var digitsAfterDecimal = ammountString.Length - decimalPointIndex - 1;

        return digitsAfterDecimal != 3;
    }
}
