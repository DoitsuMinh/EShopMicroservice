
using Catalog.API.Configurations;
using Catalog.API.Enums;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Services;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductRepository(IOptions<DbSettings> productDbSettings)
    {
        var mongoClient = new MongoClient(productDbSettings.Value.ConnectionString);

        var database = mongoClient.GetDatabase(productDbSettings.Value.DatabaseName);

        _productsCollection = database.GetCollection<Product>(productDbSettings.Value.CollectionName);
    }

    public async Task CreateAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        await _productsCollection.InsertOneAsync(product);
    }

    // Soft delete
    public async Task DeleteAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("Id cannot be null or empty", nameof(id));
        }

        var updateDefinition = Builders<Product>.Update.Set(p => p.Status, nameof(Status.Inactive));

        var result = await _productsCollection.UpdateOneAsync(
            p => p.Id == id,
            updateDefinition);
        if (result.MatchedCount == 0)
        {
            throw new KeyNotFoundException($"Product with id '{id}' not found.");
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _productsCollection.Find(p => p.Status == nameof(Status.Active)).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken)
    {
        return await _productsCollection.Find(p => p.Category == category && p.Status == nameof(Status.Active)).ToListAsync(cancellationToken);
    }

    public async Task<Product> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _productsCollection.Find(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var result = await _productsCollection.ReplaceOneAsync(p => p.Id == product.Id, product);

        if (result.MatchedCount == 0)
        {
            throw new KeyNotFoundException($"Product with id '{product.Id}' not found.");
        }
    }
}
