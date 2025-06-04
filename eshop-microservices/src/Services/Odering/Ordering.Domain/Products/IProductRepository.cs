namespace Ordering.Domain.Products;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetByIdsAsync(List<ProductId> id);
}
