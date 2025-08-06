namespace Ordering.Application.Orders;

public class ProductDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public string Name { get; set; }// Initialize with a default value

    public ProductDto()
    {
        
    }

    public ProductDto(Guid id, int quantity, string name)
    {
        Id = id;
        Quantity = quantity;
        Name = name;
    }
}
