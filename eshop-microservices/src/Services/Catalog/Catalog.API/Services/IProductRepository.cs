namespace Catalog.API.Services;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetByCategoryAsync(string name, CancellationToken cancellationToken);
    Task CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(string id);
}