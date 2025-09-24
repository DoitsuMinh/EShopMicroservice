namespace Basket.API.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    public List<CartItem> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
}
