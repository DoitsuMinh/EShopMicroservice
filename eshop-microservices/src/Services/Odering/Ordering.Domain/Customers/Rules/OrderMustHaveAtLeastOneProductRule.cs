using Ordering.Domain.Customers.Orders;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers.Rules;

internal class OrderMustHaveAtLeastOneProductRule : IBusinessRule
{
    private readonly List<OrderProductData> _orderProductsData;
    public OrderMustHaveAtLeastOneProductRule(List<OrderProductData> orderProductsData)
    {
        _orderProductsData = orderProductsData;
    }

    public string Message => "Customer must have at least one product";

    public bool IsBroken()
    {
        return _orderProductsData.Count <= 0;
    }
}