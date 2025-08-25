using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace Catalog.API.Models;

public class ProductPrice
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [Description("ProductDetailID")]
    public string ProductDetailId { get; set; } = null!;
    public decimal Price { get; set; }
    public ProductSale Sale { get; set; } = new();
    public DateTime LastUpdated { get; set; }
}

public class ProductSale
{
    public decimal SalePrice { get; set; }
    public DateTime SaleEndDate { get; set; }
}