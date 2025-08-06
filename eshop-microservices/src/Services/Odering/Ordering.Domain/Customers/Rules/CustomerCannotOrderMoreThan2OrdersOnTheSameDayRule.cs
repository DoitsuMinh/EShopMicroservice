using Ordering.Domain.Customers.Orders;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers.Rules;

public class CustomerCannotOrderMoreThan2OrdersOnTheSameDayRule : IBusinessRule
{
    private readonly IList<Order> _orders;
    public CustomerCannotOrderMoreThan2OrdersOnTheSameDayRule(IList<Order> orders)
    {
        _orders = orders;
    }

    public string Message => "Customer cannot place more than 2 orders on the same day";

    public bool IsBroken()
    {
        return _orders.Count(o => o.IsOrderedToDay()) >= 2;
    }
}