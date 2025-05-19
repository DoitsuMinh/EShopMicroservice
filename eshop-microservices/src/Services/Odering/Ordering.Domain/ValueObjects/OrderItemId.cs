namespace Ordering.Domain.ValueObjects;

public class OrderItemId
{
    public Guid Guid { get; }
    private OrderItemId(Guid guid) => Guid = guid;
    public static OrderItemId Of(Guid guid)
    {
        ArgumentNullException.ThrowIfNull(guid);
        if (guid == Guid.Empty)
            throw new DomainException("OrderItemId cannot be empty");

        return new OrderItemId(guid);
    }
}
