using BuildingBlocks.CQRS.Commands;
using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(long Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator ()
    {
        RuleFor(c => c.Id).GreaterThan(0).NotEmpty().WithMessage("Invalid Id input");
    }
}

public class DeleteProductCommandHandler(CatalogContext context)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync([command.Id], cancellationToken) ?? throw new ProductNotFoundException(command.Id);
        product.Status = false;
        context.Products.Update(product);
        await context.SaveChangesAsync(cancellationToken);
        var result = new DeleteProductResult(true);
        return result;
    }
}
