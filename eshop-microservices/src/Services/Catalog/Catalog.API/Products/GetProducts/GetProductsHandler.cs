using Catalog.API.Enums;
using Catalog.API.Services;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IProductRepository _productRepository;
    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var pageNumber = query.PageNumber ?? 1;
            var pageSize = query.PageSize ?? 10;

            var products = await _productRepository.GetAllAsync(cancellationToken);
            var result = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new GetProductsResult(products);
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

       
    }
}