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

        //RuleFor(x => x.ShoppingCart.Items)
        //    .NotEmpty()
        //    .WithMessage("Items cannot be empty");

        //RuleFor(x => x.ShoppingCart.DeliveryMethodId)
        //    .NotEmpty()
        //    .WithMessage("Delivery method cannot be empty");

        //RuleFor(x => x.ShoppingCart.ClientSecret)
        //    .NotEmpty()
        //    .WithMessage("Client secret cannot be empty");

        //RuleFor(x => x.ShoppingCart.PaymentIntentId)
        //    .NotEmpty()
        //    .WithMessage("Payment intent id cannot be empty");
    }
}

public class SetBasketCommandHandler(ICartRepository cartRepository) : ICommandHandler<SetBasketCommand, SetBasketResult>
{
    public async Task<SetBasketResult> Handle(SetBasketCommand command, CancellationToken cancellationToken)
    {
        var updatedCart = 
            await cartRepository.SetCartAsync(command.ShoppingCart) 
            ?? throw new RedisDbException("Problem writing to redis database", command.ShoppingCart.Id!); ;

        return new SetBasketResult(updatedCart);
    }
}
