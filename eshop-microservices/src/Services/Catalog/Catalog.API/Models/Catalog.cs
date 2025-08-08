namespace Catalog.API.Models;

public class Catalog : BaseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public ICollection<Category> Categories { get; set; }
}
