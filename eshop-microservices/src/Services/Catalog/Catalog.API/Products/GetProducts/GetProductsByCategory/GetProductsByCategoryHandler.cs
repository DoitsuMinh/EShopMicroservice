﻿
namespace Catalog.API.Products.GetProducts.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Categories) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

public class GetProductsByCategoryCommandHandler(IDocumentSession session)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
                                    .Where(p => p.Category.Contains(query.Categories))
                                    .ToListAsync(cancellationToken);
        
        return new GetProductsByCategoryResult(products);
    }

    private static int Factorial(int n)
    {
        if (n == 0)
            return 1;
        return n * Factorial(n - 1);
    }
}
