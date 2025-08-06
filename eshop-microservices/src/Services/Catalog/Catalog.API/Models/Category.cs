namespace Catalog.API.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int? ParentId { get; set; }
    public string ImageFile { get; set; } = default!;
    public string Status { get; set; }
}
