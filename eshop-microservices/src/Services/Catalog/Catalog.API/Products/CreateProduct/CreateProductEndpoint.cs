namespace Catalog.API.Products.CreateProduct;

<<<<<<< HEAD
public record class CreateProductRequest(string Name,
                                    List<string> Category,
                                    string Description,
                                    string ImageFile,
                                    decimal Price);
=======
public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
>>>>>>> origin/main

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreateProductResponse>();

            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}

