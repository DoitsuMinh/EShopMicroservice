using Ordering.Domain.Customers.Events;
using Ordering.Domain.Customers.Orders;
using Ordering.Domain.Customers.Rules;
using Ordering.Domain.ForeignExchange;
using Ordering.Domain.Products;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.Customers;

public class Customer : Entity, IAggregateRoot
{
    public CustomerId Id { get; private set; }

    private string _email;
    private string _name;
    private readonly List<Order> _orders;

    private Customer()
    {
        // EF Core
        _orders = [];
    }

    private Customer(string email, string name)
    {
        Id = new CustomerId(Guid.NewGuid());
        _email = email;
        _name = name;
        _orders = [];

        AddDomainEvent(new CustomerRegisteredEvent(Id));
    }

    public static Customer CreateRegistered(
        string email,
        string name,
        ICustomerUniquenessChecker customerUniquenessChecker)
    {
        CheckRule(new CustomerEmailMustBeUniqueRule(customerUniquenessChecker, email));

        return new Customer(email, name);
    }

    public OrderId PlacedOrder(
        List<OrderProductData> orderProductsData,
        List<ProductPriceData> allProductPrices,
        string currency,
        List<ConversionRate> conversionRates)
    {
        CheckRule(new OrderMustHaveAtLeastOneProductRule(orderProductsData));
        CheckRule(new CustomerCannotOrderMoreThan20OrdersOnTheSameDayRule(_orders));

        var order = Order.CreateNew(orderProductsData, allProductPrices, currency, conversionRates);
        
        _orders.Add(order);

        AddDomainEvent(new OrderPlacedEvent(order.Id, Id, order.GetValue()));

        return order.Id;
    }

    public void ChangeOrder(
        OrderId orderId,
        List<ProductPriceData> existingProducts,
        List<OrderProductData> newOrderProductsData,
        List<ConversionRate> conversionRates,
        string currency)
    {
        CheckRule(new OrderMustHaveAtLeastOneProductRule(newOrderProductsData));

        var order = _orders.Single(o => o.Id == orderId);
        order.Change(existingProducts, newOrderProductsData, conversionRates, currency);

        AddDomainEvent(new OrderChangedEvent(orderId));
    }

    public void RemoveOrder(OrderId orderId)
    {
        var order = _orders.Single(o => o.Id == orderId);
        if (order != null)
        {
            _orders.Remove(order);
            AddDomainEvent(new OrderRemovedEvent(orderId));
        }
    }
}