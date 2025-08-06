namespace Catalog.API.Models;

public class ProductDetails : BaseModel
{
    public long ProdetailId { get; set; }
    public long ProId { get; set; }
    public string ProdetailName { get; set; }
    public string Sku { get; set; }
    public Guid RefId { get; set; }
    public string ImageFile { get; set; }
    public string? Size { get; set; }
    public string Status { get; set; }
    public ProductQty ProductQty { get; set; }
}
