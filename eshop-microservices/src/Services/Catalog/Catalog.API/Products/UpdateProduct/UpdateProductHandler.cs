namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Product Product) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool isSuccess);

public class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {

        var product = await session.LoadAsync<Product>(command.Product.Id, cancellationToken);
        if (product is null)
        {
            throw new ProductNotFoundException(command.Product.Id);
        }
        product.Name = command.Product.Name;
        product.Category = command.Product.Category;
        product.Description = command.Product.Description;
        product.ImageFile = command.Product.ImageFile;
        product.Price = command.Product.Price;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}   