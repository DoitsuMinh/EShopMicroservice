//namespace Ordering.Domain.ValueObjects;

//public record CustomerId
//{
//    public long Value { get; }
//    private CustomerId(long value) => Value = value;
//    public static CustomerId Of(long value)
//    {
//        ArgumentNullException.ThrowIfNull(value);
//        if (value <= 0)
//            throw new DomainException("CustomerId cannot be empty");

//        return new CustomerId(value);
//    }
//}