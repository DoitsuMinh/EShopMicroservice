using Odering.Infrastructure.ExternalServices.FreeCurrencyApi.Models;

namespace Odering.Infrastructure.ExternalServices.FreeCurrencyApi;

public interface IFreeCurrencyApiService
{
    Task<FreeCurrencyApiResponse> GetLatestRatesAsync(FreeCurrencyApiRequest request, CancellationToken cancellationToken = default);
}