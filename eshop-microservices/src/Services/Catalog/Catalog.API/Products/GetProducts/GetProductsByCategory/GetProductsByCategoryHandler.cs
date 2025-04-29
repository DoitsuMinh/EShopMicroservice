
namespace Catalog.API.Products.GetProducts.GetProductsByCategory;

public record GetProductsByCategoryQuery(string categories) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

public class GetProductsByCategoryCommandHandler(
    IDocumentSession session,
    ILogger<GetProductsByCategoryCommandHandler> logger)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsByCategoryHandler: {Query}", query);

        var products = await session.Query<Product>()
                                    .Where(p => p.Category.Contains(query.categories))
                                    .ToListAsync(cancellationToken);
        return new GetProductsByCategoryResult(products);
    }
}
