namespace Basket.API.Models;

public class CartItem
{
    public Guid ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageFile { get; set; } = default!;
    public List<string> Category { get; set; } = [];
}