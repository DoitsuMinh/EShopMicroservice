//namespace Ordering.Domain.ValueObjects;

//public record ProductId
//{
//    public long Value { get; }
//    private ProductId(long value) => Value = value;
//    public static ProductId Of(long value)
//    {
//        ArgumentNullException.ThrowIfNull(value);
//        if (value <= 0)
//            throw new DomainException("ProductId cannot be empty");

//        return new ProductId(value);
//    }
//}