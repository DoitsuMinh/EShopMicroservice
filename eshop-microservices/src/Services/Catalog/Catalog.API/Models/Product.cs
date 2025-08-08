using Catalog.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models;

public class Product : BaseModel
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public int Category { get; set; }
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public Status Status { get; set; }
    public ICollection<ProductDetail> ProductDetails { get; set; }
}
