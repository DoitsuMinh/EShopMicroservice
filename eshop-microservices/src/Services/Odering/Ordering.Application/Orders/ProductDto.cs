namespace Ordering.Application.Orders;

public class ProductDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public string Name { get; set; } = string.Empty; // Initialize with a default value

    public ProductDto()
    {
        
    }

    public ProductDto(Guid id, int quantity)
    {
        Id = id;
        Quantity = quantity;
    }
}
