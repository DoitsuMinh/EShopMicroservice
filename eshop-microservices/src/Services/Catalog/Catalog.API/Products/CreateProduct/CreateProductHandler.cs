
using BuildingBlocks.CQRS.Commands;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name,
                                   List<string> Category,
                                   string Description,
                                   string ImageFile,
                                   decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(long Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name)
           .NotEmpty()
           .WithMessage("Name is required")
           .Length(2, 150)
           .WithMessage("Invalid Name");

        RuleFor(c => c.Category).NotEmpty().WithMessage("Category is required");

        RuleFor(c => c.Description).NotEmpty().WithMessage("Description is required");

        RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price is required");

        RuleFor(c => c.ImageFile)
        .NotEmpty()
        .Must(i => IsValidImageExtension(i))
        .WithMessage("Invalid image file");
    }

    private static bool IsValidImageExtension(string imageFile)
    {
        var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(imageFile)?.ToLowerInvariant();
        return validExtensions.Contains(extension);
    }
}

internal class CreateProductCommandHandler(CatalogContext context)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create Product entity from command object
        // save to db
        // return the result

        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };
        // save to db
        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);

        //return result
        return new CreateProductResult(product.Id);
    }
}

