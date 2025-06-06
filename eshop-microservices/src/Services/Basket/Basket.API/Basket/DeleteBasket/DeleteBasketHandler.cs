using BuildingBlocks.CQRS.Commands;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string Key) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsDeleted);

public class DeleteBasketCommandHandler(ICartRepository cartRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var result = await cartRepository.DeleteCartAsync(command.Key);
        if (!result)
        {
            throw new RedisDbException("Problem remove key in redis database", command.Key);
        }
        return new DeleteBasketResult(result);
    }
}
