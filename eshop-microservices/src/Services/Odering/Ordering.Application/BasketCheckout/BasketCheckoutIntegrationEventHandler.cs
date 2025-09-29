using BuildlingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Orders;
using Ordering.Application.Orders.PlaceCustomerOrders;

namespace Ordering.Application.BasketCheckout;

public class BasketCheckoutIntegrationEventHandler
    : IConsumer<BasketCheckoutEvent>
{
    private readonly ISender _sender;

    public BasketCheckoutIntegrationEventHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        // Create Order and start order fulfillment process

        var command = MapToPlaceCustomerOrderCommand(context.Message);
        await _sender.Send(command);
    }

    private PlaceCustomerOrderCommand MapToPlaceCustomerOrderCommand(BasketCheckoutEvent message)
    {
        // Map BasketCheckoutEvent to PlaceCustomerOrderCommand
        return new PlaceCustomerOrderCommand(
            message.CustomerId,
            [
                new ProductDto(
                    new Guid("01997a13-8b27-4e70-b469-68f23ef940ae"), 1, "Casual T-Shirt"),
                new ProductDto(
                    new Guid("01997a13-8b28-439c-afc4-62bb372be845"), 2, "Denim Jeans")
            ],
            "AUD"
        );
    }
}
