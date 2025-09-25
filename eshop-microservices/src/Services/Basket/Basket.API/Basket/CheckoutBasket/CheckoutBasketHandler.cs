using Basket.API.Dtos;
using BuildingBlocks.CQRS.Commands;
using BuildlingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can not be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required.");
    }
}

public class CheckoutBasketHandler 
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CheckoutBasketHandler(ICartRepository cartRepository, IPublishEndpoint publishEndpoint)
    {
        _cartRepository = cartRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        // Get existing basket with total price
        // Set totalprice on basketcheckout event message
        // Send basket checkout event to rabbitmq using masstransit
        // Delete the basket

        // Step 1
        var basket = await _cartRepository.GetCartAsync(command.BasketCheckoutDto.UserName, cancellationToken);
        if (basket is null)
        {
            return new CheckoutBasketResult(false);
        }

        // Step 2
        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        // Step 3
        await _publishEndpoint.Publish(eventMessage, cancellationToken);

        // Step 4
        await _cartRepository.DeleteCartAsync(command.BasketCheckoutDto.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}
