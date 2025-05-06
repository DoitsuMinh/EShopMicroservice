using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CacheCartService(IDistributedCache _cache) : ICartRepository
{
    public async Task<bool> DeleteCartAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key);
        return true;
    }

    public async Task<ShoppingCart?> GetCartAsync(string key, CancellationToken cancellationToken = default)
    {
        var cacheBasket = await _cache.GetStringAsync(key, cancellationToken);
        if (!string.IsNullOrEmpty(cacheBasket))
        {
            return JsonConvert.DeserializeObject<ShoppingCart>(cacheBasket);
        } else
        {
            throw new CartNotFoundException(key);
        }
    }

    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart, CancellationToken cancellationToken = default)
    {
        var data = JsonConvert.SerializeObject(cart);
        await _cache.SetStringAsync(cart.Id!, data, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        }, cancellationToken);
        return await GetCartAsync(cart.Id, cancellationToken);
    }
}
