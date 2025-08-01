using MediatR;
using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Application.Configuration.Data;
using Ordering.Application.Orders.PlaceCustomerOrders;
using Ordering.Domain.Customers;
using Ordering.Domain.Customers.Orders;
using Ordering.Domain.ForeignExchange;
using Ordering.Domain.Products;
using Ordering.Domain.SeedWork;

namespace Ordering.Application.Orders.ChangeCustomerOrder;

public class ChangeCustomerOrderCommandHandler : ICommandHandler<ChangeCustomerOrderCommand, Unit>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IForeignExchange _foreignExchange;

    public ChangeCustomerOrderCommandHandler(ICustomerRepository customerRepository, ISqlConnectionFactory sqlConnectionFactory, IForeignExchange foreignExchange)
    {
        _customerRepository = customerRepository;
        _sqlConnectionFactory = sqlConnectionFactory;
        _foreignExchange = foreignExchange;
    }

    public async Task<Unit> Handle(ChangeCustomerOrderCommand command, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(new CustomerId(command.CustomerId));

        var orderId = new OrderId(command.OrderId);

        var allProductPrices = await ProductPriceProvider.GetAllProductPricesAsync(_sqlConnectionFactory.GetOpenConnection());

        var conversionRates = await _foreignExchange.GetConversionRatesAsync();

        var orderProducts = command
                .Products
                .Select(x => new OrderProductData(new ProductId(x.Id), x.Quantity))
                .ToList();

        customer.ChangeOrder(
            orderId,
            allProductPrices,
            orderProducts,
            conversionRates,
            command.Currency);

        return Unit.Value;
    }
}
