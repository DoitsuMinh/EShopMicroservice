using Catalog.API.Enums;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler (CatalogDBContext context)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var pageNumber = query.PageNumber ?? 1;
            var pageSize = query.PageSize ?? 10;

            var products = await context.Product
                .Where(p => p.Status == Status.Active)
                .Select(p => new Product
                {
                    Name = p.Name,
                    Category = p.Category,
                    Description = p.Description,
                    ImageFile = p.ImageFile,
                    Price = p.Price,
                    Id = p.Id,
                    Status = p.Status,
                    CreatedDate = p.CreatedDate,
                    UpdatedDate = p.UpdatedDate
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new GetProductsResult(products);
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

       
    }
}