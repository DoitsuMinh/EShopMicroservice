using BuildingBlocks.CQRS.Commands;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Product Product) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(cmd => cmd.Product.Id)
            .NotEmpty().WithMessage("Product Id is required.");

        RuleFor(cmd => cmd.Product.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 150).WithMessage("Invalid Name");

        RuleFor(cmd => cmd.Product.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(cmd => cmd.Product.ImageFile)
            .NotEmpty()
            .Must(x => IsValidImageExtension(x))
            .WithMessage("Invalid image file");

        RuleFor(cmd => cmd.Product.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
    }

    private static bool IsValidImageExtension(string imageFile)
    {
        var validExtensions = new[] { ".jpg", ".jpeg", ".png"};
        var extension = Path.GetExtension(imageFile)?.ToLowerInvariant();
        return validExtensions.Contains(extension);
    }
}

public class UpdateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Product.Id, cancellationToken) ?? throw new ProductNotFoundException(command.Product.Id);
        product.Name = command.Product.Name;
        product.Category = command.Product.Category;
        product.Description = command.Product.Description;
        product.ImageFile = command.Product.ImageFile;
        product.Price = command.Product.Price;
        product.Status = command.Product.Status;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        var result = new UpdateProductResult(true);
        return result;
    }
}
