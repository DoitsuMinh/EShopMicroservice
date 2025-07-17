using System.Text.Json;
using Odering.Infrastructure.ExternalServices.FreeCurrencyApi.Models;
using Serilog;

namespace Odering.Infrastructure.ExternalServices.FreeCurrencyApi;

public class FreeCurrencyApiService : IFreeCurrencyApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public FreeCurrencyApiService(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<FreeCurrencyApiResponse> GetLatestRatesAsync(FreeCurrencyApiRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var currencies = string.Join(",", request.Currencies);
            var url = $"latest?apikey={request.ApiKey}&base_currency={request.BaseCurrency}&currencies={currencies}";
            
            _logger.Information("Fetching currency rates from Free Currency API for base currency {BaseCurrency} and currencies {Currencies}", 
                request.BaseCurrency, request.Currencies);
            
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<FreeCurrencyApiResponse>(jsonContent, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });
            
            _logger.Information("Successfully retrieved currency rates from Free Currency API");
            
            return result ?? new FreeCurrencyApiResponse();
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(ex, "HTTP request failed while fetching currency rates from Free Currency API. Status: {StatusCode}", ex.Data["StatusCode"]);
            throw new InvalidOperationException("Failed to retrieve currency rates from external API.", ex);
        }
        catch (TaskCanceledException ex)
        {
            _logger.Error(ex, "Request timeout while fetching currency rates from Free Currency API");
            throw new InvalidOperationException("Request timeout while retrieving currency rates from external API.", ex);
        }
        catch (JsonException ex)
        {
            _logger.Error(ex, "Failed to deserialize response from Free Currency API");
            throw new InvalidOperationException("Invalid response format from external API.", ex);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error occurred while fetching currency rates from Free Currency API");
            throw new InvalidOperationException("Failed to retrieve currency rates from external API.", ex);
        }
    }
}