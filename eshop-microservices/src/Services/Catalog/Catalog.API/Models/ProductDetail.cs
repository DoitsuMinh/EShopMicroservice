namespace Catalog.API.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

public class ProductDetail
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required]
    public string Color { get; set; } = string.Empty;

    [Required]
    public string Size { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal SellPrice { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = default!;
}

//public class Attributes
//{
//    public string Name { get; set; } = null!;
//    public string Value { get; set; } = null!;
//}

//public class ProductImage
//{
//    public int Width { get; set; }
//    public int Height { get; set; }
//    public int Src { get; set; }
//}
