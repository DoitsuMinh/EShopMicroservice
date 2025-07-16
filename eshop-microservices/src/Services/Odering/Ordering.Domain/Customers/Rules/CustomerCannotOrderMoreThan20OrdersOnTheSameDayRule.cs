using Ordering.Domain.Customers.Orders;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers.Rules;

public class CustomerCannotOrderMoreThan20OrdersOnTheSameDayRule : IBusinessRule
{
    private readonly IList<Order> _orders;
    public CustomerCannotOrderMoreThan20OrdersOnTheSameDayRule(IList<Order> orders)
    {
        _orders = orders;
    }

    public string Message => "Customer cannot place more than 20 orders on the same day";

    public bool IsBroken()
    {
        return _orders.Count(o => o.IsOrderedToDay()) >= 20;
    }
}