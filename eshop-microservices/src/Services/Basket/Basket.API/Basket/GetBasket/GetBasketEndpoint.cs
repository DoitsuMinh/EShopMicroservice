
namespace Basket.API.Basket.GetBasket;

public record GetBasketRequest(string key);
public record GetBasketResponse(ShoppingCart ShoppingCart);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{key}", async (string key, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(key));

            var response = new GetBasketResponse(result.ShoppingCart);

            return Results.Ok(response);
        })
        .WithName("GetBasket")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Get Basket")
        .WithDescription("Get Basket");
    }
}
