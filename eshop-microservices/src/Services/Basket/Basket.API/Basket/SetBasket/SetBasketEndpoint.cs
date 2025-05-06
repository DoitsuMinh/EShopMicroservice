
namespace Basket.API.Basket.SetBasket;

public record SetBasketRequest(ShoppingCart ShoppingCart);
public record SetBasketResponse(ShoppingCart ShoppingCart);

public class SetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async(SetBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<SetBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<SetBasketResponse>();
            return Results.Created($"/basket/{response.ShoppingCart.Id}", response);
        })
        .WithName("SetBasket")
        .Produces<SetBasketResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithSummary("Set Cart")
        .WithDescription("Set Cart");
    }
}
