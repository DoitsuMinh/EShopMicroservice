namespace Basket.API.Models;

public class CartItem
{
    public string ItemName { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public int Quantity { get; set; }
    public string Color { get; set; } = default!;
    public Guid ProductId { get; set; }
}