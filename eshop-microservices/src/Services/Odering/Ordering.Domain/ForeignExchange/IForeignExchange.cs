namespace Ordering.Domain.ForeignExchange;

public interface IForeignExchange
{
    List<ConversionRate> GetConversionRates();
}
