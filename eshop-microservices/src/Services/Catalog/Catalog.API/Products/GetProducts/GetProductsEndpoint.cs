using Catalog.API.Products.GetProducts.GetProductsByCategory;

namespace Catalog.API.Products.GetProducts;

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetProductsResponse(IEnumerable<Product> Products);

public record GetProductsByCategoryRequest(string Categories);
public record GetProductsByCategoryResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (
            [AsParameters] GetProductsRequest request,
            ISender sender) =>
        {
            var query = request.Adapt<GetProductsQuery>();
            var result = await sender.Send(query);

            //var result = await sender.Send(new GetProductsQuery());

            var response = result.Adapt<GetProductsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products");

        app.MapGet("/products/{categories}", async (
            int categories,
            ISender sender) =>
        {
            var result = await sender.Send(new GetProductsByCategoryQuery(categories));
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductsByCategory")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products By Category")
        .WithDescription("Get Products By Category");

       // app.MapGet("/products/category/{category}",
       //    async (string category, ISender sender) =>
       //    {
       //        var result = await sender.Send(new GetProductsByCategoryRequest(category));

       //        var response = result.Adapt<GetProductsByCategoryResponse>();

       //        return Results.Ok(response);
       //    })
       //.WithName("GetProductByCategory")
       //.Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
       //.ProducesProblem(StatusCodes.Status400BadRequest)
       //.WithSummary("Get Product By Category")
       //.WithDescription("Get Product By Category");
    }
}