using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers;

public class CustomerId(Guid value) : TypedIdValueBase(value) { }