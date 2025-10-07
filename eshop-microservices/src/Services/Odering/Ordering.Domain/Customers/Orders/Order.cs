using Ordering.Domain.Customers.Exceptions;
using Ordering.Domain.Customers.Rules;
using Ordering.Domain.ForeignExchange;
using Ordering.Domain.Products;
using Ordering.Domain.SeedWork;
using Ordering.Domain.Shared;
using Ordering.Domain.Shared.MoneyValue;

namespace Ordering.Domain.Customers.Orders;

public class Order: Entity
{
    /// <summary>
    /// Can only be accessed by the domain layer.
    /// </summary>
    internal OrderId Id;
    private bool _isRemoved;
    private MoneyValue _value;
    private MoneyValue _valueInAUD;
    private readonly List<OrderProduct> _orderProducts = [];
    private OrderStatus _status;
    private DateTime _orderDate;
    private DateTime? _orderChangeDate;

    private Order()
    {
        _orderProducts = new List<OrderProduct>();
        _isRemoved = false;
    }

    private Order(
       List<OrderProductData> orderProductsData,
       List<ProductPriceData> allProductPrices,
       string currency,
       List<ConversionRate> conversionRates)
    {
   
        // Set the order date to the current time using the system clock  
        _orderDate = SystemClock.Now;

        // Generate a unique identifier for the order  
        Id = new OrderId(Guid.NewGuid());

        // Initialize the list of order products  
        //_orderProducts = new List<OrderProduct>();

        // Iterate through the provided order product data  
        foreach (var orderProductData in orderProductsData)
        {
            // Find the price of the product matching the product ID and currency  
            var productPrice = allProductPrices.SingleOrDefault(x => x.ProductId == orderProductData.ProductId
                                                                     && x.Price.Currency == currency)
                ?? throw new ProductOrderNotFoundException(orderProductData.ProductId);

            // Create an order product instance for the given product price and quantity  
            var orderProduct = OrderProduct.CreateForProduct(
                productPrice,
                orderProductData.Quantity,
                currency,
                conversionRates);

            // Add the created order product to the list of order products  
            _orderProducts.Add(orderProduct);
        }

        // Calculate the total value of the order in both original currency and AUD  
        CalculateOrderValue();

        // Set the initial status of the order to "Placed"  
        _status = OrderStatus.Placed;

        // Set default isRemoved
        _isRemoved = false;      
    }

    private void CalculateOrderValue()
    {
        _value = _orderProducts.Sum(x => x.Value);

        _valueInAUD = _orderProducts.Sum(x => x.ValueInAUD);
    }

    internal void Remove()
    {
        _isRemoved = true;
    }

    internal static Order CreateNew(
        List<OrderProductData> orderProductsData, 
        List<ProductPriceData> allProductPrices,
        string currency,
        List<ConversionRate> conversionRates)
    {
        return new Order(orderProductsData, allProductPrices, currency, conversionRates);
    }

    internal bool IsOrderedToDay()
    {
        return _orderDate.Date == SystemClock.Now.Date;
    }

    internal MoneyValue GetValue()
    {
        return _value;
    }

    internal void Change(
       List<ProductPriceData> allProductPrices,
       List<OrderProductData> orderProductsData,
       List<ConversionRate> conversionRates,
       string currency)
    {
        // Iterate through the updated list of order products  
        foreach (var orderProductData in orderProductsData)
        {
            // Find the product price matching the product ID and currency  
            var product = allProductPrices.SingleOrDefault(x => x.ProductId == orderProductData.ProductId &&
                                                       x.Price.Currency == currency)
                ?? throw new ProductOrderNotFoundException(orderProductData.ProductId.Value);

            // Check if the product already exists in the current order  
            var existingProductOrder = _orderProducts.SingleOrDefault(x => x.ProductId == orderProductData.ProductId)
                    ?? throw new ProductOrderNotFoundException(orderProductData.ProductId.Value);

            if (existingProductOrder != null)
            {
                // If the product exists, update its quantity and recalculate its value  
                var existingOrderProduct = _orderProducts.Single(x => x.ProductId == existingProductOrder.ProductId);
                existingOrderProduct.ChangeQuantity(product, orderProductData.Quantity, conversionRates);
            }
            else
            {                
                // If the product does not exist, create a new order product and add it to the order  
                var orderProduct = OrderProduct.CreateForProduct(product, orderProductData.Quantity, currency, conversionRates);
                _orderProducts.Add(orderProduct);
            }
        }

        // Remove products from the order that are no longer in the updated list  
        var orderProductsToCheck = _orderProducts.ToList();
        foreach (var existingProduct in orderProductsToCheck)
        {
            var product = orderProductsData.SingleOrDefault(x => x.ProductId == existingProduct.ProductId);
            if (product == null)
            {
                _orderProducts.Remove(existingProduct);
            }
        }

        // Recalculate the total value of the order in both original currency and AUD  
        CalculateOrderValue();

        // Update the timestamp for when the order was last changed  
        _orderChangeDate = SystemClock.Now;
    }
}