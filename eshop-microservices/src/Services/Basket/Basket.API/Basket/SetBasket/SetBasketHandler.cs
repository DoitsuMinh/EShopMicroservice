using BuildingBlocks.CQRS.Commands;
using Discount.Grpc;

namespace Basket.API.Basket.SetBasket;

public record SetBasketCommand(ShoppingCart ShoppingCart) : ICommand<SetBasketResult>;

public record SetBasketResult(ShoppingCart ShoppingCart);

public class SetBasketCommandValidator : AbstractValidator<SetBasketCommand>
{
    public SetBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart.Id)
            .NotNull()
            .WithMessage("Invalid shopping cart");

        RuleFor(x => x.ShoppingCart.Items)
            .NotEmpty()
            .WithMessage("Items cannot be empty");

        RuleFor(x => x.ShoppingCart.DeliveryMethodId)
            .NotEmpty()
            .WithMessage("Delivery method cannot be empty");

        RuleForEach(x => x.ShoppingCart.Items)
            .Must(item => item.Quantity >= 1)
            .WithMessage("Quantity must be greater than one");
    }
}

public class SetBasketCommandHandler(
    ICartRepository cartRepository, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient) 
    : ICommandHandler<SetBasketCommand, SetBasketResult>
{
    public async Task<SetBasketResult> Handle(SetBasketCommand command, CancellationToken cancellationToken)
    {
        // TODO: communicate with Discount.Grpc and calculate latest price of products in the baseket
        await DeductDiscount(command.ShoppingCart, cancellationToken);

        // Store basket in redis
        var updatedCart = 
            await cartRepository.SetCartAsync(command.ShoppingCart) 
            ?? throw new RedisDbException("Problem writing to redis database", command.ShoppingCart.Id!);

        return new SetBasketResult(updatedCart);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest
            {
                ProductName = item.Name
            }, cancellationToken: cancellationToken);

            if (coupon.Amount >= item.Price)
            {
                item.Price = 0;
            } else
            {
                item.Price -= coupon.Amount;
            }            
        }
    }
}
