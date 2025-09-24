using Catalog.API.Helpers;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler (IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {

        // get all products with paging result
        var products = await session.Query<Product>()
            .Where(p => p.Status)
            .OrderBy(p => p.Name)
            .Page(query.PageNumber ?? 1, query.PageSize ?? 10)
            .ToListAsync(cancellationToken);

        return new GetProductsResult(products);
    }
}