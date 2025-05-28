namespace Catalog.API.Models
{
    public class Product
    {
        
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public List<string> Category { get; set; } = [];
        public string Description { get; set; } = default!;
        public string ImageFile { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public bool Status { get; set; } = true;
    }
}
