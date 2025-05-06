
namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string key): IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart ShoppingCart);
public class GetBasketQueryValidator : AbstractValidator<GetBasketQuery>
{
    public GetBasketQueryValidator()
    {
        RuleFor(x => x.key)
            .NotEmpty().WithMessage("Key is required.");
    }
}

public class GetBasketRequestHandler(ICartRepository cartRepository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await cartRepository.GetCartAsync(request.key);
        if (basket is null)
        {
            throw new CartNotFoundException(request.key);
        }
        return new GetBasketResult(basket);
    }
}
