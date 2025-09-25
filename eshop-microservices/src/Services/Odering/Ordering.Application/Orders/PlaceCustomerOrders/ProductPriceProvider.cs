using Dapper;
using Ordering.Domain.Products;
using Ordering.Domain.Shared.MoneyValue;
using System.Data;

namespace Ordering.Application.Orders.PlaceCustomerOrders;

public static class ProductPriceProvider
{
    public static async Task<List<ProductPriceData>> GetAllProductPricesAsync(IDbConnection connection)
    {
        const string sql = $"SELECT " +
            $"\"ProductId\" AS {nameof(ProductPriceResponse.ProductId)}, " +
            $"\"Value\"     AS {nameof(ProductPriceResponse.Value)}, " +
            $"\"Currency\"  AS {nameof(ProductPriceResponse.Currency)} " +
            $"FROM orders.v_productprices";

        var productPrices = await connection.QueryAsync<ProductPriceResponse>(sql);

        return productPrices
            .ToList()
            .Select(x => new ProductPriceData(
                new ProductId(x.ProductId),
                MoneyValue.Of(x.Value, x.Currency))).ToList();
    }

    private sealed class ProductPriceResponse
    {
        public Guid ProductId { get; set; }
        public decimal Value { get; set; }
        public string Currency { get; set; } = default!;
    }
}