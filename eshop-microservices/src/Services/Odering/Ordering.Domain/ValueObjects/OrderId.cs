namespace Ordering.Domain.ValueObjects;

public record OrderId
{
    public long Value { get; }
    private OrderId(long value) => Value = value;

    public static OrderId Of(long value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value <= 0)
            throw new DomainException("OrderId cannot be empty");

        return new OrderId(value);
    }
}