using Basket.API.Models;

namespace Basket.API.Data;

public interface ICartRepository
{
    Task<ShoppingCart?> SetCartAsync(ShoppingCart cart);
    Task<ShoppingCart?> GetCartAsync(string key);
    Task<bool> DeleteCartAsync(string key);
}
