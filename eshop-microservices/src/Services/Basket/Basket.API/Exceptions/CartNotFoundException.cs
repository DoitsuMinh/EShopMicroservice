using BuildingBlocks.Exceptions;

namespace Basket.API.Exceptions;

public class CartNotFoundException(string key) : NotFoundException("Cart", key)
{
}
