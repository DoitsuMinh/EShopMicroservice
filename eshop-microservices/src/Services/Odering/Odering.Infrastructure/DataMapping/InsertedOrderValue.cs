using Ordering.Domain.Models;

namespace Odering.Infrastructure.DataMapping;

public class InsertedOrderValue
{
    public long CustomerId { get; set; }
    //public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public string BillingAddress_AddressLine { get; set; } = default!;
    public string BillingAddress_Country { get; set; } = default!;
    public string BillingAddress_EmailAddress { get; set; } = default!;
    public string BillingAddress_FirstName { get; set; } = default!;
    public string BillingAddress_LastName { get; set; } = default!;
    public string BillingAddress_State { get; set; } = default!;
    public string BillingAddress_ZipCode { get; set; } = default!;
    public string OrderName { get; set; } = default!;
    public string Payment_CVV { get; set; } = default!;
    public string Payment_CardName { get; set; } = default!;
    public string Payment_CardNumber { get; set; } = default!;
    public string Payment_Expiration { get; set; } = default!;
    public int Payment_PaymentMethod { get; set; }
    public string ShippingAddress_AddressLine { get; set; } = default!;
    public string ShippingAddress_Country { get; set; } = default!;
    public string ShippingAddress_EmailAddress { get; set; } = default!;
    public string ShippingAddress_FirstName { get; set; } = default!;
    public string ShippingAddress_LastName { get; set; } = default!;
    public string ShippingAddress_State { get; set; } = default!;
    public string ShippingAddress_ZipCode { get; set; } = default!;

    public static InsertedOrderValue MapFrom(Order order)
    {
        if (isValidOrder(order))
        {
            throw new ArgumentException("Invalid order data", nameof(order));
        }
        return new InsertedOrderValue
        {
            CustomerId = order.CustomerId.Value,
            //Status = order.Status.ToString(),
            TotalPrice = order.Price,
            BillingAddress_AddressLine = order.BillingAddress.AddressLine,
            BillingAddress_Country = order.BillingAddress.Country,
            BillingAddress_EmailAddress = order.BillingAddress.EmailAddress ?? "",
            BillingAddress_FirstName = order.BillingAddress.FirstName,
            BillingAddress_LastName = order.BillingAddress.LastName,
            BillingAddress_State = order.BillingAddress.State,
            BillingAddress_ZipCode = order.BillingAddress.ZipCode,
            OrderName = order.OrderName.Value,
            Payment_CVV = order.Payment.CVV,
            Payment_CardName = order.Payment.CardName ?? "",
            Payment_CardNumber = order.Payment.CardNumber,
            Payment_Expiration = order.Payment.Expiration,
            Payment_PaymentMethod = order.Payment.PaymentMethod,
            ShippingAddress_AddressLine = order.ShippingAddress.AddressLine,
            ShippingAddress_Country = order.ShippingAddress.Country,
            ShippingAddress_EmailAddress = order.ShippingAddress.EmailAddress ?? "",
            ShippingAddress_FirstName = order.ShippingAddress.FirstName,
            ShippingAddress_LastName = order.ShippingAddress.LastName,
            ShippingAddress_State = order.ShippingAddress.State,
            ShippingAddress_ZipCode = order.ShippingAddress.ZipCode
        };
    }

    private static bool isValidOrder(Order order)
    {
        if (
            string.IsNullOrEmpty(order.Price.ToString()) ||
            string.IsNullOrEmpty(order.BillingAddress.EmailAddress) ||
            string.IsNullOrEmpty(order.ShippingAddress.EmailAddress) ||
            string.IsNullOrEmpty(order.Payment.CardName)
            )
        {
            return true;
        }
        return false;
    }
}
