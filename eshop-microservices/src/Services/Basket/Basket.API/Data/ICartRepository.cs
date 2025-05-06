namespace Basket.API.Data;

public interface ICartRepository
{
    Task<ShoppingCart?> SetCartAsync(ShoppingCart cart, CancellationToken cancellationToken = default);
    Task<ShoppingCart?> GetCartAsync(string key, CancellationToken cancellationToken = default);
    Task<bool> DeleteCartAsync(string key, CancellationToken cancellationToken = default);
}
