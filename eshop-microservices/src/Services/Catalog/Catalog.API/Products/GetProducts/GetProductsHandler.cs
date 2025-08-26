using Catalog.API.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10, string? SearchTerm ="" ) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IMongoCollection<Product> _products;
    public GetProductsQueryHandler(IMongoDatabase database, IOptions<DbSettings> settings)
    {
        _products = database.GetCollection<Product>(settings.Value.CollectionName);
    }
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {

        var pageNumber = query.PageNumber ?? 1;
        var pageSize = query.PageSize ?? 10;

        var filterBuilder = Builders<Product>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(query.SearchTerm))
        {
            var searchFilter = filterBuilder.Or(
                filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(query.SearchTerm, "i"))
                );
            filter = filterBuilder.And(filter, searchFilter);
        }

        var totalCount = await _products.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var skip = (pageNumber - 1) * pageSize;
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Get paged results
        var products = await _products.Find(filter)
                                      .Skip(skip)
                                      .Limit(pageSize)
                                      .ToListAsync(cancellationToken);


        return new GetProductsResult(products);

    }
}