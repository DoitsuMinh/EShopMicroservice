namespace Ordering.Domain.ForeignExchange;

public interface IForeignExchange
{
    Task<List<ConversionRate>> GetConversionRatesAsync();
}
