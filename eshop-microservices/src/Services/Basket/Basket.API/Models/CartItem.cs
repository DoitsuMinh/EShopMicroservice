namespace Basket.API.Models;

public class CartItem
{
    public long Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageFile { get; set; } = default!;
    public List<string> Category { get; set; } = [];
}