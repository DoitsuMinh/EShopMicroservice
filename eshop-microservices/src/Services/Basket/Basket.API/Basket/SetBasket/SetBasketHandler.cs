using BuildingBlocks.CQRS.Commands;
using Discount.Grpc;

namespace Basket.API.Basket.SetBasket;

public record SetBasketCommand(ShoppingCart ShoppingCart) : ICommand<SetBasketResult>;

public record SetBasketResult(ShoppingCart ShoppingCart);

public class SetBasketCommandValidator : AbstractValidator<SetBasketCommand>
{
    public SetBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart.UserName)
            .NotNull()
            .WithMessage("Invalid shopping cart");

        RuleFor(x => x.ShoppingCart.Items)
            .NotEmpty()
            .WithMessage("Items cannot be empty");


        RuleForEach(x => x.ShoppingCart.Items)
            .Must(item => item.Quantity > 0).WithMessage("Invalid item quantity")
            .Must(item => !string.IsNullOrEmpty(item.ItemName)).WithMessage("Invalid item name")
            .Must(item => item.Price >= 0).WithMessage("Invalid item price")
            .Must(item => item.Color != null && item.Color != " ").WithMessage("Invalid item color")
            .Must(item => item.ProductId != Guid.Empty).WithMessage("Invalid ProductId");
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
            ?? throw new RedisDbException("Problem writing to redis database", command.ShoppingCart.UserName!);

        return new SetBasketResult(updatedCart);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        var tasks = cart.Items.Select(async item =>
        {
            var coupon = await discountProtoServiceClient.GetDiscountAsync(
                                new GetDiscountRequest { ProductName = item.ItemName },
                                cancellationToken: cancellationToken);

            item.Price = coupon.Amount >= item.Price
                ? 0
                : item.Price - coupon.Amount;
        });

        await Task.WhenAll(tasks);
    }
}
