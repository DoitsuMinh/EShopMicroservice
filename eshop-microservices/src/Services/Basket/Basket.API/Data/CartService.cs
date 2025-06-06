﻿using Redis = StackExchange.Redis;

namespace Basket.API.Data;

public class CartService(Redis.IConnectionMultiplexer redis)
{
    private readonly Redis.IDatabase _database = redis.GetDatabase();
    public async Task<bool> DeleteCartAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<ShoppingCart?> GetCartAsync(string key, CancellationToken cancellationToken = default)
    {
        var cart = await _database.StringGetAsync(key);
        return cart.IsNullOrEmpty
            ? null
            : JsonConvert.DeserializeObject<ShoppingCart>(cart!);
    }

    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart, CancellationToken cancellationToken = default)
    {
        var createdCart = await _database.StringSetAsync(
                            cart.Id,
                            JsonConvert.SerializeObject(cart),
                            expiry: TimeSpan.FromHours(1));

        if (!createdCart) return null;
        
        return await GetCartAsync(cart.Id!);
    }
}
