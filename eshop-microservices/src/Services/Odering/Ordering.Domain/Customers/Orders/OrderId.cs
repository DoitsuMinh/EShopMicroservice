using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers.Orders;

public class OrderId(Guid value) : TypedIdValueBase(value) { }