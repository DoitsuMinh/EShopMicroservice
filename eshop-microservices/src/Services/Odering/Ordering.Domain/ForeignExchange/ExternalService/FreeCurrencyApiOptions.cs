namespace Ordering.Domain.ForeignExchange.ExternalService;

public class FreeCurrencyApiOptions
{
    public const string SectionName = "FreeCurrencyApi";

    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string BaseCurrency { get; set; } = string.Empty;
    public List<string> SupportedCurrencies { get; set; } = [];
    public int TimeoutSeconds { get; set; }
}
