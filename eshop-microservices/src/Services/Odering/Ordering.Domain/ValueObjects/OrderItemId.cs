namespace Ordering.Domain.ValueObjects;

public class OrderItemId
{
    public long Id { get; }
    private OrderItemId(long id) => Id = id;
    public static OrderItemId Of(long value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value <= 0)
            throw new DomainException("OrderItemId cannot be empty");

        return new OrderItemId(value);
    }
}
