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

public class DeleteProductCommandHandler(IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken) ?? throw new ProductNotFoundException(command.Id);
        product.Status = false;
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        var result = new DeleteProductResult(true);
        return result;
    }
}
