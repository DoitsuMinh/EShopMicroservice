using BuildingBlocks.CQRS.Commands;
using Catalog.API.Services;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(string Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator ()
    {
        RuleFor(c => c.Id).NotEmpty().WithMessage("Invalid Id input");
    }
}

public class DeleteProductCommandHandler
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        await _productRepository.DeleteAsync(command.Id);
        var result = new DeleteProductResult(true);
        return result;
    }
}
