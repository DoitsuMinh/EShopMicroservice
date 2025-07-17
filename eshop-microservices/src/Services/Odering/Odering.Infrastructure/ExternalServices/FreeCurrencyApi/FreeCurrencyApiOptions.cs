namespace Odering.Infrastructure.ExternalServices.FreeCurrencyApi;

public class FreeCurrencyApiOptions
{
    public const string SectionName = "FreeCurrencyApi";
    
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.freecurrencyapi.com/v1/";
    public string BaseCurrency { get; set; } = "USD";
    public List<string> SupportedCurrencies { get; set; } = new() { "AUD", "EUR", "GBP", "CAD", "JPY" };
    public int TimeoutSeconds { get; set; } = 30;
}