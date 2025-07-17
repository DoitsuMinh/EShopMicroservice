namespace Odering.Infrastructure.ExternalServices.FreeCurrencyApi.Models;

public class FreeCurrencyApiResponse
{
    public Dictionary<string, decimal> Data { get; set; } = [];
}

public class FreeCurrencyApiRequest
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseCurrency { get; set; } = "USD";
    public List<string> Currencies { get; set; } = [];
}