using Microsoft.EntityFrameworkCore;
using Odering.Infrastructure.Database;
using Odering.Infrastructure.SeedWork;
using Ordering.Domain.Products;

namespace Odering.Infrastructure.Domain.Products;

public class ProductRepository : IProductRepository
{
    private readonly OrdersContext _context;

    public ProductRepository(OrdersContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        var result = await _context.Products.IncludePaths(ProductEntityTypeConfiguration.ProductPrices).ToListAsync();
        return result;
    }

    public async Task<List<Product>> GetByIdsAsync(List<ProductId> ids)
    {
        var result = await _context.Products.IncludePaths(ProductEntityTypeConfiguration.ProductPrices)
                                            .Where(x => ids.Contains(x.Id))
                                            .ToListAsync();
        return result;
    }
}
