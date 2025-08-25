using Catalog.API.Services;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(long Id) :IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(query.Id.ToString(), cancellationToken);
        return product is null 
            ? throw new ProductNotFoundException(query.Id) 
            : new GetProductByIdResult(product);
    }
}

