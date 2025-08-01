using Ordering.Domain.Customers.Orders;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers.Rules;

public class OrderCannotHaveDuplicateProductRule : IBusinessRule
{
    private readonly List<OrderProductData> _orderProductsData;

    public OrderCannotHaveDuplicateProductRule(List<OrderProductData> orderProductsData)
    {
        _orderProductsData = orderProductsData;
    }

    public string Message => "Order cannot have duplicate product";

    public bool IsBroken()
    {
        var result = _orderProductsData
            .GroupBy(x => x.ProductId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        return result.Count > 0;
    }
}
