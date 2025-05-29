using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Odering.Infrastructure.Extensions;

internal class InitialData
{
    public static IEnumerable<Customer> Customers => new List<Customer>
    {
        Customer.Create("dev1 vu", "vu@gmail.cc"),
        Customer.Create("dev2 pham", "pham@gmail.cc"),
    };

    public static IEnumerable<Product> Products => new List<Product>
    {
        Product.Create("Tactical Combat Boots", 149.99m),
        Product.Create("Night Vision Goggles", 2499.00m),
        Product.Create("Camouflage Uniform Set", 89.99m),
        Product.Create("Field Radio Transceiver", 499.00m),
    };

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of("dev1", "vu", "vu@gmail.cc", "123 Main St", "Hanoi", "Vietnam", "10000");
            var address2 = Address.Of("dev2", "pham", "pham@gmail.cc", "543 Main St", "HCM City", "Vietnam", "10000");

            var payment1 = Payment.Of("dev1", "5555555555554444", "12/28", "123", 1);
            var payment2 = Payment.Of("dev2", "8885555555554444", "06/30", "101", 2);

            var order1 = Order.Create(
                CustomerId.Of(1003),
                OrderName.Of("ORD_1"),
                shippingAddress: address1,
                billingAddress: address1,
                payment1);

            var order2 = Order.Create(
                CustomerId.Of(1004),
                OrderName.Of("ORD_2"),
                shippingAddress: address2,
                billingAddress: address2,
                payment2);

            return new List<Order>
            {
                order1, order2
            };
        }
    }
}
