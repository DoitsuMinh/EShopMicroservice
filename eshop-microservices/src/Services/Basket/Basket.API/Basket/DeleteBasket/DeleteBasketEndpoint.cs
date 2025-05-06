namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketRequest(string Key);
public record DeleteBasketResponse(bool IsDeleted);
public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket", async ([FromBody] DeleteBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteBasketCommand>();
            var result = await sender.Send(command);
            var response = new DeleteBasketResponse(result.IsDeleted);
            return Results.Ok(response);
        })
        .WithName("DeleteBasket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Delete Basket")
        .WithDescription("Delete Basket");
    }
}
