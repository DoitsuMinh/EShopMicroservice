namespace Ordering.Domain.ForeignExchange.ExternalService;

public class FreeCurrencyApiResponse
{
    public Dictionary<string, decimal> Data { get; set; } = [];
}

public class FreeCurrencyApiRequest
{
    public FreeCurrencyApiRequest(string apiKey, string baseCurrency, List<string> currencies)
    {
        ApiKey = apiKey;
        BaseCurrency = baseCurrency;
        Currencies = currencies ?? [];
    }
    public string ApiKey { get; } = string.Empty;
    public string BaseCurrency { get; } = "USD";
    public List<string> Currencies { get; } = [];
    public string URL
    {
        get
        {
            return $"latest?apikey={ApiKey}&base_currency={BaseCurrency}&currencies={string.Join(",", Currencies)}";           
        }
    }
}

