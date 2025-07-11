using Ordering.Domain.ForeignExchange;

namespace Odering.Infrastructure.Domain.ForeignExchanges;

public class ConversionRatesCache
{
    public List<ConversionRate> Rates { get; set; } = [];
    public ConversionRatesCache(List<ConversionRate> rates)
    {
        Rates = rates;
    }
}