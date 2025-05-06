using BuildingBlocks.Exceptions;

namespace Basket.API.Exceptions;

public class RedisDbException(string name, string key): InternalServerException(name, key)
{
}
