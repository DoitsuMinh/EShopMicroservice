using BuildingBlocks.Exceptions;

namespace Basket.API.Exceptions;

public class RedisDbException(string key): InternalServerException("Problem writing to redis database", key)
{
}
