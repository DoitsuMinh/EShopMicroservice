using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models;

public class ProductDetail : BaseModel
{
    [Key]
    public long ProdetailId { get; set; }
    public long ProductId { get; set; }
    public string Sku { get; set; }
    public Guid RefId { get; set; }
    
    public string? Size { get; set; }
    public string Status { get; set; }
    public Product Product { get; set; }
    public ProductQty ProductQty { get; set; }
}
