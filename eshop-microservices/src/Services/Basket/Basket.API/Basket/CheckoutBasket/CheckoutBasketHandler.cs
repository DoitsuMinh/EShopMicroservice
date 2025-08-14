using Basket.API.Dtos;
using BuildingBlocks.CQRS.Commands;
using BuildlingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) 
    : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto)
            .NotNull().WithMessage("BasketCheckoutDto is required.");
        
        RuleFor(x => x.BasketCheckoutDto.UserName)
            .NotEmpty().WithMessage("UserName is required.");
        
        RuleFor(x => x.BasketCheckoutDto.CustomerId)
            .NotEqual(Guid.Empty).WithMessage("CustomerId is required.");
        
        RuleFor(x => x.BasketCheckoutDto.TotalPrice)
            .GreaterThan(0).WithMessage("TotalPrice must be greater than zero.");
        
        // Add more validation rules as needed
    }
}

public class CheckoutBasketHandler (ICartRepository _cartRepository, IPublishEndpoint _publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        // get existing basket with total price
        // set totalprice to basketcheckout event message
        // send basket checkout event to RabbitMQ using MassTransit
        // delete basket from Redis

        var basket = await _cartRepository.GetCartAsync(command.BasketCheckoutDto.UserName);
        if (basket is null)
        {
            return new CheckoutBasketResult(false);
        }

        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(eventMessage, cancellationToken);

        await _cartRepository.DeleteCartAsync(command.BasketCheckoutDto.UserName);

        return new CheckoutBasketResult(true);
    }
}



