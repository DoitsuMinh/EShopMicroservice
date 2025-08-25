
using Catalog.API.Services;

namespace Catalog.API.Products.GetProducts.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Categories) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

public class GetProductsByCategoryCommandHandler
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    private readonly IProductRepository _productRepository;

    public GetProductsByCategoryCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetByCategoryAsync(query.Categories, cancellationToken);

        return new GetProductsByCategoryResult(products);
    }

    private static int Factorial(int n)
    {
        if (n == 0)
            return 1;
        return n * Factorial(n - 1);
    }
}
